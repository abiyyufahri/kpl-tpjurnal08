using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace modul8_103022300121
{
    public class BankTransferConfig
    {
        public class Transfer
        {
            public int threshold { set; get; } = 25000000;
            public int low_fee { set; get; } = 6500;
            public int high_fee { set; get; } = 15000;

        }

        public class Confirmation
        {
            public string en { set; get; } = "yes";
            public string id { set; get; } = "ya";

        }
        public class BankTransfer
        {
            public string lang { set; get; } = "en";
            public Transfer transfer { set; get; }
            public List<string> methods { set; get; } = [ "RTO(real - time)", "SKN", "RTGS", "BI FAST"];
            public Confirmation confirmation { set; get; }
        }

        private BankTransfer _config;
        private readonly string _configPath = "bank_transfer_config.json";
        public BankTransferConfig()
        {
            _config = LoadData();
        }


        public void runProgram()
        {
            Console.WriteLine(_config.lang == "en" ? "Please insert the amount of money to transfer:" : "Masukkan jumlah uang yang akan di-transfer:");
            int amount = int.Parse(Console.ReadLine());
            var transferCost = amount <= _config.transfer.threshold ? _config.transfer.low_fee : _config.transfer.high_fee;
            Console.WriteLine(_config.lang == "en" ? $"Transfer fee = {transferCost} and Total amount = {amount + transferCost}" : 
                $"“Biaya t ransfer = {transferCost} dan “Total biaya = {amount  + transferCost}");
            Console.WriteLine(_config.lang == "en" ? "Select transfer method:" : "Pilih metode transfer:");
            int i = 1;
            foreach(var data in _config.methods)
            {

                Console.WriteLine($"{i} {data}");
                i++;
            }
            Console.WriteLine(_config.lang == "en" ? $"Please type {_config.confirmation.en} to confirm the transaction:" : $"Ketik {_config.confirmation.id} untuk mengkonfirmasi transaksi:");
            string answare = Console.ReadLine();

            if (_config.lang == "en" && answare == "yes" || _config.lang == "id" && answare == "ya")
            {
                Console.WriteLine(_config.lang == "en" ? "The transfer is completed" : "Proses transfer berhasil");
            }
            else
            {
                Console.WriteLine(_config.lang == "en" ? "Transfer is cancelled" : "Transfer dibatalkan");
            }

        }

        public BankTransfer LoadData()
        {
            if (File.Exists(_configPath))
            {
                try
                {
                    return JsonSerializer.Deserialize<BankTransfer>(File.ReadAllText(_configPath));
                }
                catch(Exception e)
                {
                    Console.WriteLine("Error" + e);
                    return new BankTransfer();
                }
            }
            else
            {
                var defConfig = new BankTransfer();
                SaveData(defConfig);
                return defConfig;
            }
        }
        public void SaveData(BankTransfer config)
        {
            config = config ?? _config;
            try {
                string json = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_configPath, json);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
