﻿using Microsoft.AspNetCore.Mvc;
using WeatherViewer.Pages;
using WeatherViewer.Repositories;

namespace WeatherViewer.Controllers
{
    [Route("[controller]/[action]")]
    public class ReadController : Controller
    {
        private WeatherArchiveRecordsRepository _recordsRepository;
        private const int pageSize = 7;

        public ReadController(WeatherArchiveRecordsRepository recordsRepository)
        {
            _recordsRepository = recordsRepository;
        }

        public async Task<IActionResult> Index(int pageNumber = 1, int month = 0, int year = 0)
        {
            var tableModel = new TableViewModel();

            if (month < 0 || month > 12)
            {
                tableModel.Message += "Некорректно указан месяц!\n";
                month = 0;
            }

            if (year < 0 || year > DateTime.Now.Year)
            {
                tableModel.Message += "Некорректно указан год!\n";
                year = 0;
            }

            var (records, totalCount) = await _recordsRepository.GetAllByPageModel(pageNumber, pageSize, month, year);

            tableModel.Records = records;
            tableModel.PageInfo = new PageInfo(totalCount, pageNumber, pageSize);
            tableModel.FilterInfo = new FilterInfo() { Month = month, Year = year };

            return View("~/Pages/Table.cshtml", tableModel);
        }
    }
}
