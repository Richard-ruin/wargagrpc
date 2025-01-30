namespace WargaGrpc.Models;

public class WargaModel
{
    public int IdWarga { get; set; }
    public string Nik { get; set; } = string.Empty;
    public string NamaLengkap { get; set; } = string.Empty;
    public string TempatLahir { get; set; } = string.Empty;
    public string JenisKelamin { get; set; } = string.Empty;
    public string Alamat { get; set; } = string.Empty;
}