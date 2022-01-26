using Microsoft.AspNetCore.Mvc;
using MvcCoreAzureTablesStorage.Models;
using MvcCoreAzureTablesStorage.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcCoreAzureTablesStorage.Controllers
{
    public class TableStorageController : Controller
    {
        private ServiceTableStorage service;

        public TableStorageController(ServiceTableStorage service)
        {
            this.service = service;
        }

        public async Task<IActionResult> Index()
        {
            List<Cliente> clientes = await this.service.GetClientesAsync();
            return View(clientes);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Cliente cliente)
        {
            await this.service.CrearClienteAsync(cliente.IdCLiente, cliente.Empresa, cliente.Nombre, cliente.Edad);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(string rowkey, string partitionkey)
        {
            Cliente cliente = await this.service.FindClienteAsync(rowkey, partitionkey);
            return View(cliente);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Cliente cliente)
        {
            await this.service.UpdateClienteAsync(cliente.RowKey, cliente.PartitionKey
                , cliente.Nombre, cliente.Edad);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(string rowkey, string partitionkey)
        {
            await this.service.DeleteClienteAsync(rowkey, partitionkey);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(string rowkey, string partitionkey)
        {
            Cliente cliente = await this.service.FindClienteAsync(rowkey, partitionkey);
            return View(cliente);
        }

        public IActionResult ClientesEmpresa()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ClientesEmpresa(string empresa)
        {
            List<Cliente> clientes = await this.service.GetClientesEmpresaAsync(empresa);
            return View(clientes);
        }
    }
}
