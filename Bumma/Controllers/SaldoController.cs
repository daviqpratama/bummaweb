using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using Bumma.Models;

public class SaldoController : Controller
{
    private readonly string _connectionString;

    public SaldoController(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public IActionResult Index()
    {
        var list = new List<Saldo>();
        using (var conn = new SqlConnection(_connectionString))
        {
            conn.Open();
            var cmd = new SqlCommand("SELECT * FROM saldo", conn);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Saldo
                {
                    Id = (int)reader["id"],
                    NamaAkun = reader["nama_akun"].ToString(),
                    Debit = (decimal)reader["debit"],
                    Kredit = (decimal)reader["kredit"]
                });
            }
        }
        return View(list);
    }

    public IActionResult Create() => View();

    [HttpPost]
    public IActionResult Create(Saldo saldo)
    {
        using (var conn = new SqlConnection(_connectionString))
        {
            conn.Open();
            var cmd = new SqlCommand("INSERT INTO saldo (nama_akun, debit, kredit) VALUES (@NamaAkun, @Debit, @Kredit)", conn);
            cmd.Parameters.AddWithValue("@NamaAkun", saldo.NamaAkun);
            cmd.Parameters.AddWithValue("@Debit", saldo.Debit);
            cmd.Parameters.AddWithValue("@Kredit", saldo.Kredit);
            cmd.ExecuteNonQuery();
        }
        return RedirectToAction("Index");
    }

    public IActionResult Edit(int id)
    {
        Saldo saldo = null;
        using (var conn = new SqlConnection(_connectionString))
        {
            conn.Open();
            var cmd = new SqlCommand("SELECT * FROM saldo WHERE id = @Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);
            var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                saldo = new Saldo
                {
                    Id = (int)reader["id"],
                    NamaAkun = reader["nama_akun"].ToString(),
                    Debit = (decimal)reader["debit"],
                    Kredit = (decimal)reader["kredit"]
                };
            }
        }
        return View(saldo);
    }

    [HttpPost]
    public IActionResult Edit(Saldo saldo)
    {
        using (var conn = new SqlConnection(_connectionString))
        {
            conn.Open();
            var cmd = new SqlCommand("UPDATE saldo SET nama_akun = @NamaAkun, debit = @Debit, kredit = @Kredit WHERE id = @Id", conn);
            cmd.Parameters.AddWithValue("@Id", saldo.Id);
            cmd.Parameters.AddWithValue("@NamaAkun", saldo.NamaAkun);
            cmd.Parameters.AddWithValue("@Debit", saldo.Debit);
            cmd.Parameters.AddWithValue("@Kredit", saldo.Kredit);
            cmd.ExecuteNonQuery();
        }
        return RedirectToAction("Index");
    }

    public IActionResult Delete(int id)
    {
        Saldo saldo = null;
        using (var conn = new SqlConnection(_connectionString))
        {
            conn.Open();
            var cmd = new SqlCommand("SELECT * FROM saldo WHERE id = @Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);
            var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                saldo = new Saldo
                {
                    Id = (int)reader["id"],
                    NamaAkun = reader["nama_akun"].ToString(),
                    Debit = (decimal)reader["debit"],
                    Kredit = (decimal)reader["kredit"]
                };
            }
        }
        return View(saldo);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeleteConfirmed(int id)
    {
        using (var conn = new SqlConnection(_connectionString))
        {
            conn.Open();
            var cmd = new SqlCommand("DELETE FROM saldo WHERE id = @Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.ExecuteNonQuery();
        }
        return RedirectToAction("Index");
    }
}
