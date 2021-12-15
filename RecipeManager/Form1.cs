using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using RecipeManager.BLL.Services;
using RecipeManager.BLL.Entities;
using RecipeManager.BLL.Interfaces;
using RecipeManager.DAL.Interfaces;
using RecipeManager.Core.Entities;
using System.IO;
using NLog;
using RecipeManager.DAL.DTO;
using RecipeManager.BLL.Entities.RecipesCodes;
using RecipeManager.DAL.DataContext;

namespace RecipeManager
{
    public partial class Form1 : Form
    {
        #region Constants
        const short START_CELL = 50;
        const short STEP = 210;
        const short COUNT_CELL = 130;
        const short MAX_CELL = 30770;

        const short LOW_RECIPE_DAY = 1;
        const short HIGH_RECIPE_DAY = 130;
        const short STATUS_VALUE_DAY = 1;


        #endregion

        #region Fields
        private readonly ITxtParser _txtParser = null;
        private readonly ICodesTransformator _codeTransformator = null;
        private readonly IPLCCellsChecker _pLCCellsChecker = null;
        private readonly IPLCDataTransceiver _pLCDataTransceiver = null;
        private readonly ILogger _nlog = null;
        private readonly IJsonRepository _jsonRepository = null;
        private readonly IPLCDataReciver _pLCDataReciver = null;
        private readonly IArrayToCodesConverter _codesConverter = null;
        private readonly IDoseRepository _doseRepository = null;
        private readonly IReportsRepository _reportsRepository = null;
        private readonly IReportMaker _reportMaker = null;
        private readonly AppSettingsDTO _appSettingsDTO;
        private List<RecipeCodes> _codesRecipesBuffer = null;
        private List<Recipe> _recipes = null;
        private List<CellPLC> _cells = null;

        private List<Recipe> _allRecipes = null;
        private List<RecipeCodes> _codesAllRecipesBuffer = null;
        private List<Codes> _codesForDownload = null;
        private List<CellPLC> _cellsRecipes = null;
        private List<Recipe> _nightRecipes = null;
        private List<RecipeCodes> _codesNightRecipesBuffer = null;
        private List<CellPLC> _cellsNightRecipes = null;
        private List<Recipe> _simpleRecipes = null;
        private List<RecipeCodes> _codesSimpleRecipesBuffer = null;
        private List<CellPLC> _cellsSimpleRecipes = null;

        private string _filePath = null;
        private bool _blockFlag = false;
        private bool _firstReportWas = false;
        private bool _secondReportWas = false;
        #endregion

        #region Constructors
        public Form1(AppSettingsDTO appSettingsDTO, ITxtParser txtParser, ICodesTransformator codeTransformator, IPLCCellsChecker pLCCellsChecker,
            IPLCDataTransceiver pLCDataTransceiver, IPLCDataReciver pLCDataReciver,
            IArrayToCodesConverter codesConverter, IDoseRepository doseRepository, IReportsRepository reportsRepository, IReportMaker reportMaker)
        {
            InitializeComponent();
            _txtParser = txtParser ?? throw new ArgumentNullException(nameof(txtParser));
            _codeTransformator = codeTransformator ?? throw new ArgumentNullException(nameof(codeTransformator));
            _pLCCellsChecker = pLCCellsChecker ?? throw new ArgumentNullException(nameof(pLCCellsChecker));
            _pLCDataTransceiver = pLCDataTransceiver ?? throw new ArgumentNullException(nameof(pLCDataTransceiver));
            _nlog = LogManager.GetCurrentClassLogger();
            _appSettingsDTO = appSettingsDTO;
            _pLCDataReciver = pLCDataReciver ?? throw new ArgumentNullException(nameof(pLCDataReciver));
            _codesConverter = codesConverter ?? throw new ArgumentNullException(nameof(codesConverter));
            _doseRepository = doseRepository ?? throw new ArgumentNullException(nameof(doseRepository));
            _reportsRepository = reportsRepository ?? throw new ArgumentNullException(nameof(reportsRepository));
            _reportMaker = reportMaker ?? throw new ArgumentNullException(nameof(reportMaker));
            _cellsRecipes = CellGenerator(LOW_RECIPE_DAY, HIGH_RECIPE_DAY);
            //_cellsNightRecipes = CellGenerator(LOW_RECIPE_NIGHT, HIGH_RECIPE_NIGHT);
            //_cellsSimpleRecipes = CellGenerator(LOW_RECIPE_SIMPLE, HIGH_RECIPE_SIMPLE);
            _nlog.Info("Запуск приложения");
            CountRecipeGetLabel.Text = Convert.ToString(0);
            CountDayDownloadLabel.Text = Convert.ToString(0);

            progressBar1.Visible = false;

        }

        #endregion

        #region Methods

        private void DownloadFromFileToBuffer_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                _filePath = openFileDialog1.FileName;
            }
            else return;

            var fileStr = new List<string>();

            var stream = new StreamReader(_filePath);
            while (!stream.EndOfStream)
            {
                fileStr.Add(stream.ReadLine() + "\n");
            }

            var resultFromTxt = _txtParser.ParseTxtWithResult(_filePath);
            if (!resultFromTxt.IsError)
            {
                _recipes = resultFromTxt.recipes;
                _nlog.Info($"Считывание рецептов Ок!");
            }
            else
            {
                _nlog.Info($"При считывании рецептов ошибка! Последний корректный рецепт: {resultFromTxt.Message}");
                MessageBox.Show($"При считывании рецептов произошла ошибка!\n Последний корректный рецепт: {resultFromTxt.Message} \n Проверьте структуру следующего за ним рецепта и повторите загрузку!\n", "Системное сообщение", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _allRecipes = ParseRangeRecipes(_recipes, 1, 130);

            var resultDayCodes = _codeTransformator.GetCodesWithResult(_allRecipes);
            _codesAllRecipesBuffer = resultDayCodes.codesRecipes;
            _codesForDownload = resultDayCodes.codes;

            richTextBox1.Clear();
            StatusLabel.Text = "Считывание рецептов из файла ...";
            StatusLabel.Update();
            TxtToRichTextBox(richTextBox1, _allRecipes, progressBar1);
            StatusLabel.Text = "";
            StatusLabel.Update();
            MessageBox.Show($"Рецепты успешно считаны!\n Количество считанных рецептов: {_recipes.Count()}", "Системное сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
            CountRecipeGetLabel.Text = _allRecipes.Count().ToString();
        }

        private void DownLoadDayBufferToPLC_Click(object sender, EventArgs e)
        {
            if (!_blockFlag)
            {
                _blockFlag = true;
                DialogResult dialogResult = DialogResult.Cancel;
                int countDownloadedRecipes = 0;
                var listsCodes = MakeListsCodes(_codesAllRecipesBuffer);
                //var realCells = _pLCCellsChecker.GetCellStatusByRangeWithResult(_cellsRecipes);
                dialogResult = MessageBox.Show($"Все незаконченные рецепты будут перезаписанны.\n Продолжить?", "Системное сообщение", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (dialogResult == DialogResult.OK)
                {
                    var result = _pLCDataTransceiver.SetDataWithResult(listsCodes, _codesForDownload, _cellsRecipes, STATUS_VALUE_DAY);
                    if(result.IsError)
                    {
                        _nlog.Error(result.Message);
                        MessageBox.Show($"Нет связи с ПЛК!\n Обратитесь к администратору!", "Системное сообщение", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    var countRecipes = _recipes.Count;
                    if (!result.IsError)
                    {
                        richTextBox1.Clear();
                        StatusLabel.Text = "Загрузка рецептов в ПЛК ...";
                        StatusLabel.Update();
                        TxtToRichTextBox(richTextBox1, _allRecipes, progressBar1);
                        CountDayDownloadLabel.Text = countDownloadedRecipes.ToString();
                        StatusLabel.Text = "";
                        StatusLabel.Update();
                        MessageBox.Show($"Запись {listsCodes.Count} рецептов прошла успешно!", "Системное сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        _blockFlag = false;
                    }
                    else
                    {
                        _nlog.Error($"Ошибка при записи в ПЛК{result.Message}");
                        MessageBox.Show($"Ошибка при записи рецептов!\n Обратитесь к администратору!", "Системное сообщение", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        _blockFlag = false;
                        return;
                    }

                }
                else
                {
                    _blockFlag = false;
                    return;
                }
            }
            else
            {
                MessageBox.Show($"Идет считыванеи данных, повторите попытку позже!", "Системное сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }


        private List<Recipe> ParseRangeRecipes(List<Recipe> recipes, short lowLine, short highLine)
        {

            var result = new List<Recipe>();

            for (short i = (short)(lowLine - 1); i < highLine; i++)
            {
                if (i < recipes.Count)
                {
                    if (recipes[i] != null)
                    {
                        result.Add(recipes[i]);
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }

            }
            return result;
        }

        private List<List<short>> MakeListsCodes(List<RecipeCodes> recipesBuffer)
        {
            var listGeneral = new List<List<short>>();
            foreach (var codes in recipesBuffer)
            {
                var list = new List<short>();
                list.AddRange(codes.RecipeNameCodes);
                list.Add(codes.ProductCount_LowWord);
                list.Add(codes.ProductCount_HighWord);


                foreach (var comp in codes.ComponentsCodes)
                {
                    list.AddRange(comp.Comp_NameCodes);
                    list.Add(comp.Comp_Code);
                    list.Add(comp.Comp_Weight_LowWord);
                    list.Add(comp.Comp_Weight_HighWord);
                }
    
                listGeneral.Add(list);
            }

            return listGeneral;

        }

        private void TxtToRichTextBox(RichTextBox richTextBox, List<Recipe> allRecipes, ProgressBar progressBar)
        {
            int strPointer = 0;
            int recipeCounter = 1;
            string str, status = null;
            Color color = Color.Black;
            progressBar.Maximum = allRecipes.Count;
            progressBar.Visible = true;

            foreach (var recipe in allRecipes)
            {
                progressBar.Value = recipeCounter;

                if (recipe.DownloadStatusFlag == true)
                {
                    color = Color.Green;
                    status = $"Статус: Загружен\n";
                }
                else
                {
                    color = Color.Red;
                    status = $"Статус: Незагружен\n";
                }

                MakeRecipeHeader(richTextBox1, recipe.RecipeName, recipe.ProductCount, ref strPointer, ref recipeCounter);

                str = status;
                richTextBox.Select(strPointer, str.Length);
                richTextBox.SelectionFont = new Font("Verdana", 8, FontStyle.Bold);
                richTextBox.SelectionColor = color;
                richTextBox.AppendText(str);
                strPointer = strPointer + str.Length;

                MakeComponentsList(richTextBox1, recipe.Components, ref strPointer);

                recipeCounter++;
            }

            progressBar.Visible = false;

        }

        private void MakeRecipeHeader(RichTextBox richTextBox, string Name, int Count, ref int strPointer, ref int RecipeCounter)
        {
            //str = $"Рецепт: {recipe.RecipeName}\n  Количество:{recipe.ProductCount}\n  Код оператора:{recipe.CodeOperator}\n Задержка:{recipe.RecipeDelay} \n";

            string str = $"{RecipeCounter}) Рецепт: ";

            richTextBox.Select(strPointer, str.Length);
            richTextBox.SelectionFont = new Font("Verdana", 12, FontStyle.Bold);
            richTextBox.SelectionColor = Color.Blue;
            richTextBox.AppendText(str);
            strPointer = strPointer + str.Length;

            string str1 = $"{Name}\n";

            richTextBox.Select(strPointer, str1.Length);
            richTextBox.SelectionFont = new Font("Verdana", 10, FontStyle.Bold);
            richTextBox.SelectionColor = Color.Black;
            richTextBox.AppendText(str1);
            strPointer = strPointer + str1.Length;

            string str2 = "Количество: ";

            richTextBox.Select(strPointer, str2.Length);
            richTextBox.SelectionFont = new Font("Verdana", 10, FontStyle.Bold);
            richTextBox.SelectionColor = Color.Blue;
            richTextBox.AppendText(str2);
            strPointer = strPointer + str2.Length;

            string str3 = $"{Count}\n";

            richTextBox.Select(strPointer, str3.Length);
            richTextBox.SelectionFont = new Font("Verdana", 10, FontStyle.Bold);
            richTextBox.SelectionColor = Color.Black;
            richTextBox.AppendText(str3);
            strPointer = strPointer + str3.Length;


        }

        private void MakeComponentsList(RichTextBox richTextBox, List<RecipeManager.Core.Entities.Component> components, ref int strPointer)
        {
            foreach (var component in components)
            {
                string str = null;
                if (component.Comp_Name == "0")
                {
                    str = $"Код: { component.Comp_Code}\n Без названия\n Вес: {component.Comp_Weight}\n ";
                }
                else
                {
                   
                   str = $"Код: {component.Comp_Code}\n {component.Comp_Name}\n  Вес: {component.Comp_Weight / 1000.00} кг\n";
                }

                richTextBox.Select(strPointer, str.Length);
                richTextBox.SelectionColor = Color.Black;
                richTextBox.AppendText(str);
                strPointer = strPointer + str.Length;
            }
        }
       

        private void CellGeneratorRandom()
        {
            for (short i = 0; i < COUNT_CELL; i++)
            {
                var cell = new CellPLC();
                cell.CellAddress = (short)(START_CELL + (STEP * i));
                if (i % 2 == 0) { cell.IsEmpty = false; }
                else { cell.IsEmpty = true; }
                _cells.Add(cell);

            }
        }

        private void CellGenerator()
        {
            for (short i = 0; i < COUNT_CELL; i++)
            {
                var cell = new CellPLC();
                cell.CellAddress = (short)(START_CELL + (STEP * i));
                if (i % 2 == 0) { cell.IsEmpty = false; }
                else { cell.IsEmpty = false; }
                _cells.Add(cell);

            }
        }

        private List<CellPLC> CellGenerator(short lowRangeCell, short highRangeCell)
        {
            var listCell = new List<CellPLC>();
            for (short i = (short)(lowRangeCell - 1); i < highRangeCell; i++)
            {
                var cell = new CellPLC();
                cell.CellAddress = (short)(START_CELL + (STEP * i));
                if (i % 2 == 0) { cell.IsEmpty = false; }
                else { cell.IsEmpty = false; }
                listCell.Add(cell);

            }
            return listCell;
        }

        private void ClearDownloadsFlagsRecipes()
        {
            foreach (var recipe in _allRecipes)
            {
                recipe.DownloadStatusFlag = false;
            }
        }

      

        #endregion

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                Hide();

                notifyIcon1.BalloonTipTitle = "Программа была спрятана";
                notifyIcon1.BalloonTipText = "Обратите внимание, что программа была спрятана и продолжает свою работу.";
                notifyIcon1.ShowBalloonTip(5000); // Параметром указываем количество миллисекунд, которое будет показываться подсказка
            }
            
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }
    }
}
