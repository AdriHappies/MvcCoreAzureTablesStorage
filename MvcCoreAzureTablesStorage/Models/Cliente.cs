using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcCoreAzureTablesStorage.Models
{
    public class Cliente: TableEntity
    {
        //ROWKEY: IDCLIENTE
        //CUANDO ESCRIBAN EL ID DE CLIENTE, ALMACENAMOS EL ROWKEY
        private string _IdCliente;
        public string IdCLiente
        {
            get { return this._IdCliente; }
            set {
                this._IdCliente = value;
                this.RowKey = value;
            }
        }
        //PARTITIO KEY: EMRESA
        //CUANDO ESCRIBAN LA EMPRESA, ALMACENAMOS EL PARTITION KEY
        private string _Empresa;
        public string Empresa
        {
            get { return this._Empresa; }
            set
            {
                this._Empresa = value;
                this.PartitionKey = value;
            }
        }

        public string Nombre { get; set; }
        public string Edad { get; set; }
    }
}
