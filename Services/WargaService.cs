using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using WargaGrpc.Data;
using WargaGrpc.Models;

namespace WargaGrpc.Services;

public class WargaService : WargaGrpc.WargaService.WargaServiceBase
{
    private readonly WargaContext _context;
    private readonly ILogger<WargaService> _logger;

    public WargaService(WargaContext context, ILogger<WargaService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public override async Task<WargaList> GetAllWarga(Empty request, ServerCallContext context)
    {
        var response = new WargaList();
        var wargaList = await _context.Warga.ToListAsync();

        foreach (var warga in wargaList)
        {
            response.Items.Add(MapToProto(warga));
        }

        return response;
    }

    public override async Task<Warga> GetWarga(WargaId request, ServerCallContext context)
    {
        var warga = await _context.Warga.FindAsync(request.IdWarga);
        if (warga == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, $"Warga with ID {request.IdWarga} not found"));
        }

        return MapToProto(warga);
    }

    public override async Task<Warga> CreateWarga(Warga request, ServerCallContext context)
    {
        var warga = new WargaModel
        {
            Nik = request.Nik,
            NamaLengkap = request.NamaLengkap,
            TempatLahir = request.TempatLahir,
            JenisKelamin = request.JenisKelamin,
            Alamat = request.Alamat
        };

        _context.Warga.Add(warga);
        await _context.SaveChangesAsync();

        return MapToProto(warga);
    }

    public override async Task<Warga> UpdateWarga(Warga request, ServerCallContext context)
    {
        var warga = await _context.Warga.FindAsync(request.IdWarga);
        if (warga == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, $"Warga with ID {request.IdWarga} not found"));
        }

        warga.Nik = request.Nik;
        warga.NamaLengkap = request.NamaLengkap;
        warga.TempatLahir = request.TempatLahir;
        warga.JenisKelamin = request.JenisKelamin;
        warga.Alamat = request.Alamat;

        await _context.SaveChangesAsync();

        return MapToProto(warga);
    }

    public override async Task<Empty> DeleteWarga(WargaId request, ServerCallContext context)
    {
        var warga = await _context.Warga.FindAsync(request.IdWarga);
        if (warga == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, $"Warga with ID {request.IdWarga} not found"));
        }

        _context.Warga.Remove(warga);
        await _context.SaveChangesAsync();

        return new Empty();
    }

    private static Warga MapToProto(WargaModel model)
    {
        return new Warga
        {
            IdWarga = model.IdWarga,
            Nik = model.Nik,
            NamaLengkap = model.NamaLengkap,
            TempatLahir = model.TempatLahir,
            JenisKelamin = model.JenisKelamin,
            Alamat = model.Alamat
        };
    }
}