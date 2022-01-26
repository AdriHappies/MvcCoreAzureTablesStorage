 using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;
using MvcCoreAzureTablesStorage.Models;

namespace MvcCoreAzureTablesStorage.Services
{
    public class ServiceTableStorage
    {
        private CloudTable tablaClientes;
        private ServiceTableStorage(string keys)
        {
            CloudStorageAccount account = CloudStorageAccount.Parse(keys);
            CloudTableClient client = account.CreateCloudTableClient();
            this.tablaClientes = client.GetTableReference("clientes");
            this.tablaClientes.CreateIfNotExists();
        }

        //METODO PARA CREAR CLIENTES EN LA TABLA
        public async Task CrearClienteAsync(string id, string empresa, string nombre, string edad)
        {
            Cliente cliente = new Cliente();
            cliente.IdCLiente = id;
            cliente.Empresa = empresa;
            cliente.Nombre = nombre;
            cliente.Edad = edad;
            //LAS CONSULTAS DE ACCION SE REALIZAN MEDIANTE OBJETOS DE TIPO TableOperation
            //Y SON EJECUTADAS POSTERIORMENTE SOBRE CADA TABLA
            TableOperation insert = TableOperation.Insert(cliente);
            await this.tablaClientes.ExecuteAsync(insert);
        }

        //METODO PARA DEVOLVER TODOS LOS CLIENTES
        public async Task<List<Cliente>> GetClientesAsync()
        {
            //PARA RECUPERAR LOS ELEMENTOS DE LA TABLA DEBEMOS UTILIZAR UN OBJETO TableQuery<T>
            TableQuery<Cliente> query = new TableQuery<Cliente>();
            //LA CONSULTA EN STORAGE TABLES LA REALIZA POR SEGMENTOS PARA IR TRANSFORMANDO
            //JSON EN CLASES
            TableQuerySegment<Cliente> segment = 
                await this.tablaClientes.ExecuteQuerySegmentedAsync(query, null);
            List<Cliente> clientes = segment.Results;
            return clientes;
        }

        //METODO PARA BUSCAR POR EMPRESA
        public async Task<List<Cliente>> GetClientesEmpresaAsync(string empresa)
        {
            TableQuery<Cliente> query = new TableQuery<Cliente>();
            TableQuerySegment<Cliente> segment =
                await this.tablaClientes.ExecuteQuerySegmentedAsync(query, null);
            //FILTRAMOS LOS CAMPOS QUE NECESITEMOS
            List<Cliente> clientes = segment.Where(x => x.Empresa == empresa).ToList();
            return clientes;
        }

        //METODO PARA BUSCAR POR ROWKEY
        //EN TABLES NO PODEMOS BUSCAR SOLO POR ROWKEY, DEBEMOS HACERLO EN CONJUNTO CON SU PARTITIONKEY PARA DEVOLVER UNA SOLA FILA
        public async Task<Cliente> FindClienteAsync(string rowkey, string partitionkey)
        {
            TableOperation select = TableOperation.Retrieve<Cliente>(partitionkey, rowkey);
            TableResult result = await this.tablaClientes.ExecuteAsync(select);
            Cliente cliente = result.Result as Cliente;
            return cliente;
        }

        public async Task DeleteCliente(string rowkey, string partitionkey)
        {
            Cliente cliente = await this.FindClienteAsync(rowkey, partitionkey);
            TableOperation delete = TableOperation.Delete(cliente);
            await this.tablaClientes.ExecuteAsync(delete);
        }

        public async Task UpdateCliente(string rowkey, string partitionkey, string nombre, string edad)
        {
            Cliente cliente = await this.FindClienteAsync(rowkey, partitionkey);
            cliente.Nombre = nombre;
            cliente.Edad = edad;
            TableOperation update = TableOperation.Replace(cliente);
            await this.tablaClientes.ExecuteAsync(update);
        }
    }
}
