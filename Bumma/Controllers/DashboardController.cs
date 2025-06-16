using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;  // Pastikan NuGet Microsoft.Data.SqlClient sudah terpasang
using Bumma.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace Bumma.Controllers
{
    public class DashboardController : Controller
    {
        private readonly string _connectionString;

        public DashboardController(IConfiguration configuration)
        {
            // Mengambil connection string dari appsettings.json
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }

        [HttpGet]
        public IActionResult Index()
        {
            var data = new List<Saldo>();
            decimal totalDebit = 0m;
            decimal totalKredit = 0m;

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    // Pastikan nama tabel: "saldo"
                    string query = "SELECT id, nama_akun, debit, kredit FROM saldo";

                    using (var command = new SqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Baca kolom dengan GetOrdinal untuk menghindari typo
                                int idxId = reader.GetOrdinal("id");
                                int idxNama = reader.GetOrdinal("nama_akun");
                                int idxDebit = reader.GetOrdinal("debit");
                                int idxKredit = reader.GetOrdinal("kredit");

                                var saldo = new Saldo
                                {
                                    Id = reader.GetInt32(idxId),
                                    NamaAkun = reader.GetString(idxNama),
                                    Debit = reader.GetDecimal(idxDebit),
                                    Kredit = reader.GetDecimal(idxKredit)
                                };
                                data.Add(saldo);

                                // Hitung total
                                totalDebit += saldo.Debit;
                                totalKredit += saldo.Kredit;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Logging error (misalnya ke file log atau console)
                // Untuk sementara, bisa simpan di TempData agar bisa ditampilkan di View jika diperlukan:
                TempData["DashboardError"] = $"Error saat mengambil data saldo: {ex.Message}";
                // Anda bisa juga log menggunakan ILogger<DashboardController> jika di-inject
            }

            // Simpan ringkasan ke ViewBag agar View dapat menggunakannya
            ViewBag.TotalDebit = totalDebit;
            ViewBag.TotalKredit = totalKredit;
            ViewBag.Selisih = totalDebit - totalKredit;

            // Kirim list Saldo ke View
            return View(data);
        }
    }
}
