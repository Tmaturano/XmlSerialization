using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace XMLTest
{
    public class Program
    {
        [XmlRoot("Stock")]
        public class Stock        
        {
            [XmlAttribute]
            public string Version { get; set; }  
            public Message Message { get; set; }  
            //[XmlElement("nome")]
        }

        public class Message
        {
            public Store Store { get; set; }
            public PafEcf PafEcf { get; set; }
            public StockData StockData { get; set; }
        }

        public class Store
        {            
            public string Ie { get; set; }
            public string Cnpj { get; set; }
            public string Name { get; set; }
        }

        public class PafEcf
        {
            public string CredentialNumber { get; set; }
            public string CommercialName { get; set; }
            public string Version { get; set; }
            public string DeveloperCnpj { get; set; }
            public string DeveloperName { get; set; }                
        }

        public class StockData
        {
            public string InitialDate { get; set; }
            public string EndDate { get; set; }
            public List<Product> Products { get; set; }

            public StockData()
            {
                Products = new List<Product>();
            }
        }

        public class Product
        {
            public string Description { get; set; }
            //Colocar Tipo = "GTIN"                     
            public CodeType Code { get; set; }
            public string Amount { get; set; }
            public string Unity { get; set; }
            public string UnitValue { get; set; }
            public string SituacaoTributaria { get; set; }
            public string Aliquota { get; set; }
            public string RoundingIndicator { get; set; }
            public string Ippt { get; set; }
            public string StockSituation { get; set; }
        }

        public class CodeType
        {
            [XmlAttribute]
            public EnumType Type { get; set; }
                        
            [XmlText]
            public string Code { get; set; }
        }

        public enum EnumType
        {
            [Description("GTIN")]
            GTIN,

            [Description("EAN.UCC")]
            EANUCC,

            [Description("EAN")]
            EAN,

            [Description("Proprio")]
            Proprio
        }

        
        static void Main(string[] args)
        {          

            string result = string.Empty;
            
            Store store = new Store();
            store.Ie = "123456789";
            store.Cnpj = "12345678000123";
            store.Name = "New Business";

            PafEcf pafEcf = new PafEcf();
            pafEcf.Version = "1.0";
            pafEcf.CredentialNumber = "123000000345";
            pafEcf.CommercialName = "Sample Store";            
            pafEcf.DeveloperCnpj = "87654321000321";
            pafEcf.DeveloperName = "Developer One";      
            
            StockData stockData = new StockData();
            stockData.InitialDate = DateTime.Now.ToString("dd/MM/yyyy");
            stockData.EndDate = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");

            CodeType codeType = new CodeType();
            codeType.Type = EnumType.GTIN;
            codeType.Code = "123456";

            Product product;
            product = new Product();
            product.Description = "Television";
            product.Code = codeType;
            product.Amount = "230";
            product.Unity = "Un";

            decimal value = 2900.9m;

            product.UnitValue = value.ToString("N2"); //String.Format("{0:0.00}", value);
            product.SituacaoTributaria = "T";
            product.Aliquota = "17,00".Replace(",", string.Empty) ;
            product.RoundingIndicator = "0";
            product.Ippt = "P";
            product.StockSituation = "P";

            stockData.Products.Add(product);

            product = new Product();
            product.Description = "Notebook";
            //produto.Codigo = "897654";
            product.Amount = "150";
            product.Unity = "Un";
            product.UnitValue = String.Format("{0:0.00}", "3200.659"); //String.Format("{0:.##}", "3200.569");
            product.SituacaoTributaria = "T";
            product.Aliquota = "25,00".Replace(",", string.Empty);
            product.RoundingIndicator = "0";
            product.Ippt = "P";
            product.StockSituation = "P";

            stockData.Products.Add(product);

            Message mensagem = new Message();
            mensagem.Store = store;
            mensagem.PafEcf = pafEcf;
            mensagem.StockData = stockData;

            Stock estoque = new Stock();
            estoque.Version = "1.0";
            estoque.Message = mensagem;            

            //XML Generation
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add(string.Empty, string.Empty);

            XmlSerializer xml = new XmlSerializer(estoque.GetType());            
            xml.Serialize(Console.Out, estoque, ns);

            using (MemoryStream ms = new MemoryStream())
            {
                XmlWriter writer = XmlWriter.Create(ms);
                xml.Serialize(writer, estoque, ns);
                ms.Seek(0, SeekOrigin.Begin);

                StreamReader sr = new StreamReader(ms, Encoding.UTF8);
                result = sr.ReadToEnd();
            }

            if(!Directory.Exists("C:\\Teste"))            
                Directory.CreateDirectory("C:\\Teste");

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(result);
            xmlDoc.Save("C:\\Teste\\teste.xml");
            
            //Console.WriteLine(result);
            Console.ReadKey();
        }
    }
}
