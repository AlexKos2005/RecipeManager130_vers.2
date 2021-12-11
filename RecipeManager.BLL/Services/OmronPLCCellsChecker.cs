using System;
using System.Collections.Generic;
using RecipeManager.BLL.Entities;
using RecipeManager.BLL.Interfaces;
using OmronFinsTCP.Net;
using System.Net.NetworkInformation;

namespace RecipeManager.BLL.Services
{
    public class OmronPLCCellsChecker : IPLCCellsChecker
    {
        private readonly string _ipAddress = null;
        private EtherNetPLC _etherNetPLC = null;
        private PlcMemory _plcMemory = PlcMemory.DM;

        public OmronPLCCellsChecker()
        {

        }

        public OmronPLCCellsChecker(string ipAddress)
        {
            _ipAddress = ipAddress ?? throw new ArgumentNullException(nameof(ipAddress));
        }

        public (bool IsError, string Message, List<CellPLC> Cells) GetCellStatusByRangeWithResult(List<CellPLC> Cells)
        {
            var ListCells = Cells ?? throw new ArgumentNullException(nameof(Cells));
            var resultListCell = new List<CellPLC>();
            short data = 0;
            short status = 0;

            var connectResult = ConnectInit();
            if (!connectResult.IsError)
            {
                foreach(var listCells in ListCells)
                {
                   status = _etherNetPLC.ReadWord(_plcMemory, listCells.CellAddress, out data);
                    if (status==0)
                    {
                        if (data != 0)
                        {
                            listCells.IsEmpty = false;
                        }
                        else
                        {
                            listCells.IsEmpty = true;
                        }
                        resultListCell.Add(listCells);
                    }
                    else
                    {
                        _etherNetPLC.Close();
                        return (true, $"Метод Read вернул -1, {listCells.CellAddress}", null);
                    }
                   
                }
                _etherNetPLC.Close();
                return (false, "", resultListCell);
            }
            _etherNetPLC.Close();
            return (true, connectResult.Message, null);
            
        }

        private (bool IsError, string Message) ConnectInit()
        {
            if (!PingPLC())
            {
                _etherNetPLC = new EtherNetPLC();
                _etherNetPLC.Link(_ipAddress, 9600, 1000);

                return (false, "");
            }
            else
            {
                return (true, "Не удалось подключиться к ПЛК! Отсутствует PING!");
            }

        }


        private bool PingPLC()
        {
            Ping ping = new Ping();
            PingReply rep;
            rep = ping.Send(_ipAddress, 1000);
            if (rep.Status == IPStatus.Success)//проверка статуса пинга
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
