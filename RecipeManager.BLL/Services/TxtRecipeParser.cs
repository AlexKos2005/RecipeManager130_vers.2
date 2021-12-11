using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using RecipeManager.BLL.Interfaces;
using RecipeManager.Core.Entities;

namespace RecipeManager.BLL.Services
{

    public class TxtRecipeParser : ITxtParser,IDisposable
    {
        const int RECIPE_STR_COUNT = 54;
        const int COMPONENT_COUNT = 17;
        const int COMPONENT_FOR_SKIPING = 16; //компонент перед водой лишний и всегда нулевой - пропускаем.
        private string currentRecipeName = null;

        public (bool IsError, string Message, List<Recipe> recipes) ParseTxtWithResult(string filePath)
        {
            
            var fileStr = new List<string>();
            var recipes = new List<Recipe>();
            var stream = new StreamReader(filePath);
            while (!stream.EndOfStream)
            {
                fileStr.Add(stream.ReadLine().Trim('\t'));
            }

            try
            {
                
                for (int i = 0; i < fileStr.Count / RECIPE_STR_COUNT; i++)
                {
                    int c = 0;
                    var recipe = new Recipe();
                    recipe.RecipeName = fileStr[i * RECIPE_STR_COUNT+c].ToString();
                    c++;
                    recipe.ProductCount = Convert.ToInt32(fileStr[i * RECIPE_STR_COUNT + c]);
                    c++;
                    //recipe.OperatorCode = Convert.ToInt32(fileStr[i * RECIPE_STR_COUNT + c]);
                    //c++;
                    //recipe.RecipeDelay = Convert.ToInt32(fileStr[i * RECIPE_STR_COUNT + c]);
                    //c++;

                    recipe.DownloadStatusFlag = false;

                    for (int f=0;f<COMPONENT_COUNT;f++)
                    {
                        //if(f==COMPONENT_FOR_SKIPING-1)//пропускаем 16 компонент (в текстовом файле лишний)
                        //{
                        //    c = c + 3;
                        //    continue;
                        //}
                        if(f==COMPONENT_COUNT-1)//вода
                        {
                            var water = new WaterComponent();
                            water.Comp_Code = Convert.ToInt32(fileStr[i * RECIPE_STR_COUNT + c]);
                            c++;
                            water.Comp_Name = fileStr[i * RECIPE_STR_COUNT + c];
                            c++;
                            water.Comp_Weight = Convert.ToInt32(fileStr[i * RECIPE_STR_COUNT + c]);
                            c++;
                            water.Comp_Temper = Convert.ToInt32(fileStr[i * RECIPE_STR_COUNT + c]);
                            c++;
                            recipe.WaterComp = water;
                        }
                        else //простой компонент
                        {
                            var component = new Component();
                            component.Comp_Code = Convert.ToInt32(fileStr[i * RECIPE_STR_COUNT + c]);
                            c++;
                            component.Comp_Name = fileStr[i * RECIPE_STR_COUNT + c];
                            c++;
                            component.Comp_Weight = Convert.ToInt32(fileStr[i * RECIPE_STR_COUNT + c]);
                            c++;
                            recipe.Components.Add(component);
                        } 
                    }

                    currentRecipeName = fileStr[i * RECIPE_STR_COUNT].ToString(); // если рецепт считался без ошибок, то фиксируем его название.

                    recipes.Add(recipe);
                }

                return (false, "", recipes);
            }
            catch(Exception e)
            {
                return (true,currentRecipeName, recipes);
            }
            

        }


        public void Dispose()
        {
            throw new NotImplementedException();
        }

    }
}
