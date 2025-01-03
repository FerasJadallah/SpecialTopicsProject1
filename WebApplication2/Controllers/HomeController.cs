using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using Nethereum.Web3;
using WebApplication2.Models;
//using ActionResult = System.Web.Mvc.ActionResult;
using Ipfs.Http;

using Nethereum.Util;
using System.Web;
using System.Net.Http;

namespace WebApplication2.Controllers
{

    public class HomeController : System.Web.Mvc.Controller
    {
       
        private IpfsClient ipfs = new IpfsClient("http://localhost:5001");
        private Web3 web3 = new Web3("HTTP://127.0.0.1:7545");
        [HttpPost]
        public async Task<ActionResult> Upload(HttpPostedFileBase file, string fileName)
        {
            
            using (var ms = new MemoryStream())
            {

                file.InputStream.CopyTo(ms);
                var fileBytes = ms.ToArray();

                

                var ms2 = new MemoryStream(fileBytes);
                var ipfsPath = await ipfs.FileSystem.AddAsync(ms2);
                var ipfsHash = ipfsPath.Id.Hash.ToString();
                var ethHashes1 = System.IO.File.ReadAllLines(Server.MapPath("~/App_Data/ethHashes.txt"));
                int version = 1;

                foreach (var hash in ethHashes1)
                {
                    var count = await GetFileVersion(hash, fileName);

                    
                    version = count + version;

                  
                }
                var privateKey = "0xbaf516c1648953985cb8d4b7a30e8244260696b447c9c91d4eb64771aea8a63b";
                var account = new Nethereum.Web3.Accounts.Account(privateKey);
                var web3 = new Web3(account, "http://localhost:7545");
                var contract = web3.Eth.GetContract("[\r\n\t\t\t{\r\n\t\t\t\t\"inputs\": [\r\n\t\t\t\t\t{\r\n\t\t\t\t\t\t\"internalType\": \"string\",\r\n\t\t\t\t\t\t\"name\": \"\",\r\n\t\t\t\t\t\t\"type\": \"string\"\r\n\t\t\t\t\t}\r\n\t\t\t\t],\r\n\t\t\t\t\"name\": \"fileRecords\",\r\n\t\t\t\t\"outputs\": [\r\n\t\t\t\t\t{\r\n\t\t\t\t\t\t\"internalType\": \"string\",\r\n\t\t\t\t\t\t\"name\": \"ipfsHash\",\r\n\t\t\t\t\t\t\"type\": \"string\"\r\n\t\t\t\t\t},\r\n\t\t\t\t\t{\r\n\t\t\t\t\t\t\"internalType\": \"string\",\r\n\t\t\t\t\t\t\"name\": \"fileName\",\r\n\t\t\t\t\t\t\"type\": \"string\"\r\n\t\t\t\t\t},\r\n\t\t\t\t\t{\r\n\t\t\t\t\t\t\"internalType\": \"uint256\",\r\n\t\t\t\t\t\t\"name\": \"version\",\r\n\t\t\t\t\t\t\"type\": \"uint256\"\r\n\t\t\t\t\t}\r\n\t\t\t\t],\r\n\t\t\t\t\"stateMutability\": \"view\",\r\n\t\t\t\t\"type\": \"function\"\r\n\t\t\t},\r\n\t\t\t{\r\n\t\t\t\t\"inputs\": [\r\n\t\t\t\t\t{\r\n\t\t\t\t\t\t\"internalType\": \"string\",\r\n\t\t\t\t\t\t\"name\": \"ethHash\",\r\n\t\t\t\t\t\t\"type\": \"string\"\r\n\t\t\t\t\t}\r\n\t\t\t\t],\r\n\t\t\t\t\"name\": \"retrieveFileName\",\r\n\t\t\t\t\"outputs\": [\r\n\t\t\t\t\t{\r\n\t\t\t\t\t\t\"internalType\": \"string\",\r\n\t\t\t\t\t\t\"name\": \"\",\r\n\t\t\t\t\t\t\"type\": \"string\"\r\n\t\t\t\t\t}\r\n\t\t\t\t],\r\n\t\t\t\t\"stateMutability\": \"view\",\r\n\t\t\t\t\"type\": \"function\"\r\n\t\t\t},\r\n\t\t\t{\r\n\t\t\t\t\"inputs\": [\r\n\t\t\t\t\t{\r\n\t\t\t\t\t\t\"internalType\": \"string\",\r\n\t\t\t\t\t\t\"name\": \"ethHash\",\r\n\t\t\t\t\t\t\"type\": \"string\"\r\n\t\t\t\t\t}\r\n\t\t\t\t],\r\n\t\t\t\t\"name\": \"retrieveIpfsHash\",\r\n\t\t\t\t\"outputs\": [\r\n\t\t\t\t\t{\r\n\t\t\t\t\t\t\"internalType\": \"string\",\r\n\t\t\t\t\t\t\"name\": \"\",\r\n\t\t\t\t\t\t\"type\": \"string\"\r\n\t\t\t\t\t}\r\n\t\t\t\t],\r\n\t\t\t\t\"stateMutability\": \"view\",\r\n\t\t\t\t\"type\": \"function\"\r\n\t\t\t},\r\n\t\t\t{\r\n\t\t\t\t\"inputs\": [\r\n\t\t\t\t\t{\r\n\t\t\t\t\t\t\"internalType\": \"string\",\r\n\t\t\t\t\t\t\"name\": \"ethHash\",\r\n\t\t\t\t\t\t\"type\": \"string\"\r\n\t\t\t\t\t}\r\n\t\t\t\t],\r\n\t\t\t\t\"name\": \"retrieveVersion\",\r\n\t\t\t\t\"outputs\": [\r\n\t\t\t\t\t{\r\n\t\t\t\t\t\t\"internalType\": \"uint256\",\r\n\t\t\t\t\t\t\"name\": \"\",\r\n\t\t\t\t\t\t\"type\": \"uint256\"\r\n\t\t\t\t\t}\r\n\t\t\t\t],\r\n\t\t\t\t\"stateMutability\": \"view\",\r\n\t\t\t\t\"type\": \"function\"\r\n\t\t\t},\r\n\t\t\t{\r\n\t\t\t\t\"inputs\": [\r\n\t\t\t\t\t{\r\n\t\t\t\t\t\t\"internalType\": \"string\",\r\n\t\t\t\t\t\t\"name\": \"ethHash\",\r\n\t\t\t\t\t\t\"type\": \"string\"\r\n\t\t\t\t\t},\r\n\t\t\t\t\t{\r\n\t\t\t\t\t\t\"internalType\": \"string\",\r\n\t\t\t\t\t\t\"name\": \"fileName\",\r\n\t\t\t\t\t\t\"type\": \"string\"\r\n\t\t\t\t\t},\r\n\t\t\t\t\t{\r\n\t\t\t\t\t\t\"internalType\": \"string\",\r\n\t\t\t\t\t\t\"name\": \"ipfsHash\",\r\n\t\t\t\t\t\t\"type\": \"string\"\r\n\t\t\t\t\t},\r\n\t\t\t\t\t{\r\n\t\t\t\t\t\t\"internalType\": \"uint256\",\r\n\t\t\t\t\t\t\"name\": \"version\",\r\n\t\t\t\t\t\t\"type\": \"uint256\"\r\n\t\t\t\t\t}\r\n\t\t\t\t],\r\n\t\t\t\t\"name\": \"storeFile\",\r\n\t\t\t\t\"outputs\": [],\r\n\t\t\t\t\"stateMutability\": \"nonpayable\",\r\n\t\t\t\t\"type\": \"function\"\r\n\t\t\t}\r\n\t\t]", "0xb7a450e465e48cf4c1567fc67a1586d07df5c202"); 
                var storeHashFunction = contract.GetFunction("storeFile");
                var ethHashBytes = new Sha3Keccack().CalculateHash(fileBytes);
                var ethHash = BitConverter.ToString(ethHashBytes).Replace("-", "").ToLower();
                if (ethHashes1.Contains(ethHash))
                {
                    TempData["Message"] = "File already exists on the blockchain.";
                    return RedirectToAction("FileList");
                }
                var gasPrice = await web3.Eth.GasPrice.SendRequestAsync();
                web3.TransactionManager.UseLegacyAsDefault = true;

                var gasLimit = await storeHashFunction.EstimateGasAsync(account.Address, null, null, ethHash, ipfsHash);
                
                var gas = new Nethereum.Hex.HexTypes.HexBigInteger(gasLimit.Value * 2);
                var receipt = await storeHashFunction.SendTransactionAndWaitForReceiptAsync(account.Address, gas, null, null, ethHash, fileName, ipfsHash, version);
                System.IO.File.AppendAllText(Server.MapPath("~/App_Data/ethHashes.txt"), ethHash + Environment.NewLine);

                return View("Index"); 
            }
        }
        private async Task<int> GetFileVersion(string ethHash, string fileName)
        {
           
            var count = 0;
            var privateKey = "0xbaf516c1648953985cb8d4b7a30e8244260696b447c9c91d4eb64771aea8a63b";
            var account = new Nethereum.Web3.Accounts.Account(privateKey);
            var web3 = new Web3(account, "http://localhost:7545");
            var contract = web3.Eth.GetContract("[\r\n\t\t\t{\r\n\t\t\t\t\"inputs\": [\r\n\t\t\t\t\t{\r\n\t\t\t\t\t\t\"internalType\": \"string\",\r\n\t\t\t\t\t\t\"name\": \"\",\r\n\t\t\t\t\t\t\"type\": \"string\"\r\n\t\t\t\t\t}\r\n\t\t\t\t],\r\n\t\t\t\t\"name\": \"fileRecords\",\r\n\t\t\t\t\"outputs\": [\r\n\t\t\t\t\t{\r\n\t\t\t\t\t\t\"internalType\": \"string\",\r\n\t\t\t\t\t\t\"name\": \"ipfsHash\",\r\n\t\t\t\t\t\t\"type\": \"string\"\r\n\t\t\t\t\t},\r\n\t\t\t\t\t{\r\n\t\t\t\t\t\t\"internalType\": \"string\",\r\n\t\t\t\t\t\t\"name\": \"fileName\",\r\n\t\t\t\t\t\t\"type\": \"string\"\r\n\t\t\t\t\t},\r\n\t\t\t\t\t{\r\n\t\t\t\t\t\t\"internalType\": \"uint256\",\r\n\t\t\t\t\t\t\"name\": \"version\",\r\n\t\t\t\t\t\t\"type\": \"uint256\"\r\n\t\t\t\t\t}\r\n\t\t\t\t],\r\n\t\t\t\t\"stateMutability\": \"view\",\r\n\t\t\t\t\"type\": \"function\"\r\n\t\t\t},\r\n\t\t\t{\r\n\t\t\t\t\"inputs\": [\r\n\t\t\t\t\t{\r\n\t\t\t\t\t\t\"internalType\": \"string\",\r\n\t\t\t\t\t\t\"name\": \"ethHash\",\r\n\t\t\t\t\t\t\"type\": \"string\"\r\n\t\t\t\t\t}\r\n\t\t\t\t],\r\n\t\t\t\t\"name\": \"retrieveFileName\",\r\n\t\t\t\t\"outputs\": [\r\n\t\t\t\t\t{\r\n\t\t\t\t\t\t\"internalType\": \"string\",\r\n\t\t\t\t\t\t\"name\": \"\",\r\n\t\t\t\t\t\t\"type\": \"string\"\r\n\t\t\t\t\t}\r\n\t\t\t\t],\r\n\t\t\t\t\"stateMutability\": \"view\",\r\n\t\t\t\t\"type\": \"function\"\r\n\t\t\t},\r\n\t\t\t{\r\n\t\t\t\t\"inputs\": [\r\n\t\t\t\t\t{\r\n\t\t\t\t\t\t\"internalType\": \"string\",\r\n\t\t\t\t\t\t\"name\": \"ethHash\",\r\n\t\t\t\t\t\t\"type\": \"string\"\r\n\t\t\t\t\t}\r\n\t\t\t\t],\r\n\t\t\t\t\"name\": \"retrieveIpfsHash\",\r\n\t\t\t\t\"outputs\": [\r\n\t\t\t\t\t{\r\n\t\t\t\t\t\t\"internalType\": \"string\",\r\n\t\t\t\t\t\t\"name\": \"\",\r\n\t\t\t\t\t\t\"type\": \"string\"\r\n\t\t\t\t\t}\r\n\t\t\t\t],\r\n\t\t\t\t\"stateMutability\": \"view\",\r\n\t\t\t\t\"type\": \"function\"\r\n\t\t\t},\r\n\t\t\t{\r\n\t\t\t\t\"inputs\": [\r\n\t\t\t\t\t{\r\n\t\t\t\t\t\t\"internalType\": \"string\",\r\n\t\t\t\t\t\t\"name\": \"ethHash\",\r\n\t\t\t\t\t\t\"type\": \"string\"\r\n\t\t\t\t\t}\r\n\t\t\t\t],\r\n\t\t\t\t\"name\": \"retrieveVersion\",\r\n\t\t\t\t\"outputs\": [\r\n\t\t\t\t\t{\r\n\t\t\t\t\t\t\"internalType\": \"uint256\",\r\n\t\t\t\t\t\t\"name\": \"\",\r\n\t\t\t\t\t\t\"type\": \"uint256\"\r\n\t\t\t\t\t}\r\n\t\t\t\t],\r\n\t\t\t\t\"stateMutability\": \"view\",\r\n\t\t\t\t\"type\": \"function\"\r\n\t\t\t},\r\n\t\t\t{\r\n\t\t\t\t\"inputs\": [\r\n\t\t\t\t\t{\r\n\t\t\t\t\t\t\"internalType\": \"string\",\r\n\t\t\t\t\t\t\"name\": \"ethHash\",\r\n\t\t\t\t\t\t\"type\": \"string\"\r\n\t\t\t\t\t},\r\n\t\t\t\t\t{\r\n\t\t\t\t\t\t\"internalType\": \"string\",\r\n\t\t\t\t\t\t\"name\": \"fileName\",\r\n\t\t\t\t\t\t\"type\": \"string\"\r\n\t\t\t\t\t},\r\n\t\t\t\t\t{\r\n\t\t\t\t\t\t\"internalType\": \"string\",\r\n\t\t\t\t\t\t\"name\": \"ipfsHash\",\r\n\t\t\t\t\t\t\"type\": \"string\"\r\n\t\t\t\t\t},\r\n\t\t\t\t\t{\r\n\t\t\t\t\t\t\"internalType\": \"uint256\",\r\n\t\t\t\t\t\t\"name\": \"version\",\r\n\t\t\t\t\t\t\"type\": \"uint256\"\r\n\t\t\t\t\t}\r\n\t\t\t\t],\r\n\t\t\t\t\"name\": \"storeFile\",\r\n\t\t\t\t\"outputs\": [],\r\n\t\t\t\t\"stateMutability\": \"nonpayable\",\r\n\t\t\t\t\"type\": \"function\"\r\n\t\t\t}\r\n\t\t]", "0xb7a450e465e48cf4c1567fc67a1586d07df5c202");
            var getFileNameFunction = contract.GetFunction("retrieveFileName");
            
            var FileName2 = await getFileNameFunction.CallAsync<string>(ethHash);
            if (fileName == FileName2)
            { count++; }

            return count;
        }
        public async Task<ActionResult> FileList()
        {
            if (Session["IsLoggedIn"] == null || !(bool)Session["IsLoggedIn"])
            {
                return RedirectToAction("Login");
            }
            var web3 = new Web3("http://localhost:7545");
            var ContractABI = "[\r\n\t\t\t{\r\n\t\t\t\t\"inputs\": [\r\n\t\t\t\t\t{\r\n\t\t\t\t\t\t\"internalType\": \"string\",\r\n\t\t\t\t\t\t\"name\": \"\",\r\n\t\t\t\t\t\t\"type\": \"string\"\r\n\t\t\t\t\t}\r\n\t\t\t\t],\r\n\t\t\t\t\"name\": \"fileRecords\",\r\n\t\t\t\t\"outputs\": [\r\n\t\t\t\t\t{\r\n\t\t\t\t\t\t\"internalType\": \"string\",\r\n\t\t\t\t\t\t\"name\": \"ipfsHash\",\r\n\t\t\t\t\t\t\"type\": \"string\"\r\n\t\t\t\t\t},\r\n\t\t\t\t\t{\r\n\t\t\t\t\t\t\"internalType\": \"string\",\r\n\t\t\t\t\t\t\"name\": \"fileName\",\r\n\t\t\t\t\t\t\"type\": \"string\"\r\n\t\t\t\t\t},\r\n\t\t\t\t\t{\r\n\t\t\t\t\t\t\"internalType\": \"uint256\",\r\n\t\t\t\t\t\t\"name\": \"version\",\r\n\t\t\t\t\t\t\"type\": \"uint256\"\r\n\t\t\t\t\t}\r\n\t\t\t\t],\r\n\t\t\t\t\"stateMutability\": \"view\",\r\n\t\t\t\t\"type\": \"function\"\r\n\t\t\t},\r\n\t\t\t{\r\n\t\t\t\t\"inputs\": [\r\n\t\t\t\t\t{\r\n\t\t\t\t\t\t\"internalType\": \"string\",\r\n\t\t\t\t\t\t\"name\": \"ethHash\",\r\n\t\t\t\t\t\t\"type\": \"string\"\r\n\t\t\t\t\t}\r\n\t\t\t\t],\r\n\t\t\t\t\"name\": \"retrieveFileName\",\r\n\t\t\t\t\"outputs\": [\r\n\t\t\t\t\t{\r\n\t\t\t\t\t\t\"internalType\": \"string\",\r\n\t\t\t\t\t\t\"name\": \"\",\r\n\t\t\t\t\t\t\"type\": \"string\"\r\n\t\t\t\t\t}\r\n\t\t\t\t],\r\n\t\t\t\t\"stateMutability\": \"view\",\r\n\t\t\t\t\"type\": \"function\"\r\n\t\t\t},\r\n\t\t\t{\r\n\t\t\t\t\"inputs\": [\r\n\t\t\t\t\t{\r\n\t\t\t\t\t\t\"internalType\": \"string\",\r\n\t\t\t\t\t\t\"name\": \"ethHash\",\r\n\t\t\t\t\t\t\"type\": \"string\"\r\n\t\t\t\t\t}\r\n\t\t\t\t],\r\n\t\t\t\t\"name\": \"retrieveIpfsHash\",\r\n\t\t\t\t\"outputs\": [\r\n\t\t\t\t\t{\r\n\t\t\t\t\t\t\"internalType\": \"string\",\r\n\t\t\t\t\t\t\"name\": \"\",\r\n\t\t\t\t\t\t\"type\": \"string\"\r\n\t\t\t\t\t}\r\n\t\t\t\t],\r\n\t\t\t\t\"stateMutability\": \"view\",\r\n\t\t\t\t\"type\": \"function\"\r\n\t\t\t},\r\n\t\t\t{\r\n\t\t\t\t\"inputs\": [\r\n\t\t\t\t\t{\r\n\t\t\t\t\t\t\"internalType\": \"string\",\r\n\t\t\t\t\t\t\"name\": \"ethHash\",\r\n\t\t\t\t\t\t\"type\": \"string\"\r\n\t\t\t\t\t}\r\n\t\t\t\t],\r\n\t\t\t\t\"name\": \"retrieveVersion\",\r\n\t\t\t\t\"outputs\": [\r\n\t\t\t\t\t{\r\n\t\t\t\t\t\t\"internalType\": \"uint256\",\r\n\t\t\t\t\t\t\"name\": \"\",\r\n\t\t\t\t\t\t\"type\": \"uint256\"\r\n\t\t\t\t\t}\r\n\t\t\t\t],\r\n\t\t\t\t\"stateMutability\": \"view\",\r\n\t\t\t\t\"type\": \"function\"\r\n\t\t\t},\r\n\t\t\t{\r\n\t\t\t\t\"inputs\": [\r\n\t\t\t\t\t{\r\n\t\t\t\t\t\t\"internalType\": \"string\",\r\n\t\t\t\t\t\t\"name\": \"ethHash\",\r\n\t\t\t\t\t\t\"type\": \"string\"\r\n\t\t\t\t\t},\r\n\t\t\t\t\t{\r\n\t\t\t\t\t\t\"internalType\": \"string\",\r\n\t\t\t\t\t\t\"name\": \"fileName\",\r\n\t\t\t\t\t\t\"type\": \"string\"\r\n\t\t\t\t\t},\r\n\t\t\t\t\t{\r\n\t\t\t\t\t\t\"internalType\": \"string\",\r\n\t\t\t\t\t\t\"name\": \"ipfsHash\",\r\n\t\t\t\t\t\t\"type\": \"string\"\r\n\t\t\t\t\t},\r\n\t\t\t\t\t{\r\n\t\t\t\t\t\t\"internalType\": \"uint256\",\r\n\t\t\t\t\t\t\"name\": \"version\",\r\n\t\t\t\t\t\t\"type\": \"uint256\"\r\n\t\t\t\t\t}\r\n\t\t\t\t],\r\n\t\t\t\t\"name\": \"storeFile\",\r\n\t\t\t\t\"outputs\": [],\r\n\t\t\t\t\"stateMutability\": \"nonpayable\",\r\n\t\t\t\t\"type\": \"function\"\r\n\t\t\t}\r\n\t\t]";
            var contract = web3.Eth.GetContract(ContractABI, "0xb7a450e465e48cf4c1567fc67a1586d07df5c202");
            

            var getFileFunction = contract.GetFunction("retrieveIpfsHash");
            var getFileFunction1 = contract.GetFunction("retrieveFileName");
            var getFileFunction2 = contract.GetFunction("retrieveVersion");



            var ethHashes = System.IO.File.ReadAllLines(Server.MapPath("~/App_Data/ethHashes.txt"));

            var files = new List<FileViewModel>();

            foreach (var ethHash in ethHashes)
            {
                var filehash = await getFileFunction.CallAsync<string>(ethHash);
                var filename = await getFileFunction1.CallAsync<string>(ethHash);
                var fileversion = await getFileFunction2.CallAsync<uint>(ethHash);

                files.Add(new FileViewModel
                {
                    EthHash = ethHash,
                    FileName = filename,
                    Version = fileversion
                });
            }

            return View(files);
        }
    
        [HttpGet]
        public async Task<ActionResult> Download(string ethHash)
        {
            var account = new Nethereum.Web3.Accounts.Account("0xbaf516c1648953985cb8d4b7a30e8244260696b447c9c91d4eb64771aea8a63b");
            var contract = web3.Eth.GetContract("[\r\n\t\t\t{\r\n\t\t\t\t\"inputs\": [\r\n\t\t\t\t\t{\r\n\t\t\t\t\t\t\"internalType\": \"string\",\r\n\t\t\t\t\t\t\"name\": \"\",\r\n\t\t\t\t\t\t\"type\": \"string\"\r\n\t\t\t\t\t}\r\n\t\t\t\t],\r\n\t\t\t\t\"name\": \"fileRecords\",\r\n\t\t\t\t\"outputs\": [\r\n\t\t\t\t\t{\r\n\t\t\t\t\t\t\"internalType\": \"string\",\r\n\t\t\t\t\t\t\"name\": \"ipfsHash\",\r\n\t\t\t\t\t\t\"type\": \"string\"\r\n\t\t\t\t\t},\r\n\t\t\t\t\t{\r\n\t\t\t\t\t\t\"internalType\": \"string\",\r\n\t\t\t\t\t\t\"name\": \"fileName\",\r\n\t\t\t\t\t\t\"type\": \"string\"\r\n\t\t\t\t\t},\r\n\t\t\t\t\t{\r\n\t\t\t\t\t\t\"internalType\": \"uint256\",\r\n\t\t\t\t\t\t\"name\": \"version\",\r\n\t\t\t\t\t\t\"type\": \"uint256\"\r\n\t\t\t\t\t}\r\n\t\t\t\t],\r\n\t\t\t\t\"stateMutability\": \"view\",\r\n\t\t\t\t\"type\": \"function\"\r\n\t\t\t},\r\n\t\t\t{\r\n\t\t\t\t\"inputs\": [\r\n\t\t\t\t\t{\r\n\t\t\t\t\t\t\"internalType\": \"string\",\r\n\t\t\t\t\t\t\"name\": \"ethHash\",\r\n\t\t\t\t\t\t\"type\": \"string\"\r\n\t\t\t\t\t}\r\n\t\t\t\t],\r\n\t\t\t\t\"name\": \"retrieveFileName\",\r\n\t\t\t\t\"outputs\": [\r\n\t\t\t\t\t{\r\n\t\t\t\t\t\t\"internalType\": \"string\",\r\n\t\t\t\t\t\t\"name\": \"\",\r\n\t\t\t\t\t\t\"type\": \"string\"\r\n\t\t\t\t\t}\r\n\t\t\t\t],\r\n\t\t\t\t\"stateMutability\": \"view\",\r\n\t\t\t\t\"type\": \"function\"\r\n\t\t\t},\r\n\t\t\t{\r\n\t\t\t\t\"inputs\": [\r\n\t\t\t\t\t{\r\n\t\t\t\t\t\t\"internalType\": \"string\",\r\n\t\t\t\t\t\t\"name\": \"ethHash\",\r\n\t\t\t\t\t\t\"type\": \"string\"\r\n\t\t\t\t\t}\r\n\t\t\t\t],\r\n\t\t\t\t\"name\": \"retrieveIpfsHash\",\r\n\t\t\t\t\"outputs\": [\r\n\t\t\t\t\t{\r\n\t\t\t\t\t\t\"internalType\": \"string\",\r\n\t\t\t\t\t\t\"name\": \"\",\r\n\t\t\t\t\t\t\"type\": \"string\"\r\n\t\t\t\t\t}\r\n\t\t\t\t],\r\n\t\t\t\t\"stateMutability\": \"view\",\r\n\t\t\t\t\"type\": \"function\"\r\n\t\t\t},\r\n\t\t\t{\r\n\t\t\t\t\"inputs\": [\r\n\t\t\t\t\t{\r\n\t\t\t\t\t\t\"internalType\": \"string\",\r\n\t\t\t\t\t\t\"name\": \"ethHash\",\r\n\t\t\t\t\t\t\"type\": \"string\"\r\n\t\t\t\t\t}\r\n\t\t\t\t],\r\n\t\t\t\t\"name\": \"retrieveVersion\",\r\n\t\t\t\t\"outputs\": [\r\n\t\t\t\t\t{\r\n\t\t\t\t\t\t\"internalType\": \"uint256\",\r\n\t\t\t\t\t\t\"name\": \"\",\r\n\t\t\t\t\t\t\"type\": \"uint256\"\r\n\t\t\t\t\t}\r\n\t\t\t\t],\r\n\t\t\t\t\"stateMutability\": \"view\",\r\n\t\t\t\t\"type\": \"function\"\r\n\t\t\t},\r\n\t\t\t{\r\n\t\t\t\t\"inputs\": [\r\n\t\t\t\t\t{\r\n\t\t\t\t\t\t\"internalType\": \"string\",\r\n\t\t\t\t\t\t\"name\": \"ethHash\",\r\n\t\t\t\t\t\t\"type\": \"string\"\r\n\t\t\t\t\t},\r\n\t\t\t\t\t{\r\n\t\t\t\t\t\t\"internalType\": \"string\",\r\n\t\t\t\t\t\t\"name\": \"fileName\",\r\n\t\t\t\t\t\t\"type\": \"string\"\r\n\t\t\t\t\t},\r\n\t\t\t\t\t{\r\n\t\t\t\t\t\t\"internalType\": \"string\",\r\n\t\t\t\t\t\t\"name\": \"ipfsHash\",\r\n\t\t\t\t\t\t\"type\": \"string\"\r\n\t\t\t\t\t},\r\n\t\t\t\t\t{\r\n\t\t\t\t\t\t\"internalType\": \"uint256\",\r\n\t\t\t\t\t\t\"name\": \"version\",\r\n\t\t\t\t\t\t\"type\": \"uint256\"\r\n\t\t\t\t\t}\r\n\t\t\t\t],\r\n\t\t\t\t\"name\": \"storeFile\",\r\n\t\t\t\t\"outputs\": [],\r\n\t\t\t\t\"stateMutability\": \"nonpayable\",\r\n\t\t\t\t\"type\": \"function\"\r\n\t\t\t}\r\n\t\t]", "0xb7a450e465e48cf4c1567fc67a1586d07df5c202");
            var retrieveHashFunction = contract.GetFunction("retrieveIpfsHash");
            var ipfsHash = await retrieveHashFunction.CallAsync<string>(ethHash);
            

            using (var httpClient = new HttpClient())
            {
                var fileBytes = await httpClient.GetByteArrayAsync("http://127.0.0.1:8080/ipfs/" + ipfsHash);
                return File(fileBytes, "application/octet-stream", "FromIpfs.pdf");
            }
            
        }
       

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Settings()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Logout()
        {
            Session["IsLoggedIn"] = false;


            return RedirectToAction("Index", "Home");
        }

        public ActionResult Setting()
        {
            if (Session["IsLoggedIn"] == null || !(bool)Session["IsLoggedIn"])
            {
                return RedirectToAction("Login");
            }

            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult ForTest()
        {
            return View();
        }
       



        public ActionResult Documents()
        {
            if (Session["IsLoggedIn"] == null || !(bool)Session["IsLoggedIn"])
            {
                return RedirectToAction("Login");
            }

            var dir = new DirectoryInfo(Server.MapPath("~/App_Data/uploads"));
            var files = dir.EnumerateFiles().Select(file => file.Name).ToList();

            if (files == null)
            {
                files = new List<string>();
            }

            return View(files);
        }
       
        
        [System.Web.Mvc.HttpPost]
        public ActionResult Login(string username, string password)
        {
            var edgeDriverPath = @"C:\msedgedriver.exe";

            var options = new EdgeOptions();
            options.AddArgument("--headless");
            EdgeDriverService service = EdgeDriverService.CreateDefaultService(edgeDriverPath);
            using (IWebDriver driver = new EdgeDriver(service, options))
            {
                try
                {
                    driver.Navigate().GoToUrl("https://cats.iku.edu.tr/portal");

                    var usernameField = driver.FindElement(By.Id("eid"));
                    var passwordField = driver.FindElement(By.Id("pw"));

                    usernameField.SendKeys(username);
                    passwordField.SendKeys(password);

                    var loginButton = driver.FindElement(By.Id("submit"));
                    loginButton.Click();

                    bool loginSuccess = IsLoggedIn(driver);

                    if (loginSuccess)
                    {
                        string SessionCookieKey = "SessionCookies";
                        Session[SessionCookieKey] = driver.Manage().Cookies.AllCookies;

                       
                        Session["IsLoggedIn"] = true;

                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.LoginFailure = true;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    ViewBag.LoginFailure = true; 
                }
            }

            return View();
        }

        public ActionResult Dashboard()
        {
            if (Session["IsLoggedIn"] == null || !(bool)Session["IsLoggedIn"])
            {
                return RedirectToAction("Login");
            }



            return View();
        }

        private bool IsLoggedIn(IWebDriver driver)
        {
            try
            {
                var condition = driver.FindElement(By.Id("Mrphs-sites-nav"));
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

    }
}

