using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Microsoft.EntityFrameworkCore;
using WargaGrpc.Data;
using WargaGrpc.Models;

namespace WargaGrpc.Controllers;

[ApiController]
[Route("[controller]")]
[EnableCors("AllowReactApp")]
public class WargaController : ControllerBase
{
    private readonly WargaContext _context;
    private readonly ILogger<WargaController> _logger;

    public WargaController(WargaContext context, ILogger<WargaController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<WargaModel>>> GetWarga()
    {
        return await _context.Warga.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<WargaModel>> GetWarga(int id)
    {
        var warga = await _context.Warga.FindAsync(id);

        if (warga == null)
        {
            return NotFound();
        }

        return warga;
    }

    [HttpPost]
    public async Task<ActionResult<WargaModel>> PostWarga(WargaModel warga)
    {
        _context.Warga.Add(warga);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetWarga), new { id = warga.IdWarga }, warga);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutWarga(int id, WargaModel warga)
    {
        if (id != warga.IdWarga)
        {
            return BadRequest();
        }

        _context.Entry(warga).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!WargaExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteWarga(int id)
    {
        var warga = await _context.Warga.FindAsync(id);
        if (warga == null)
        {
            return NotFound();
        }

        _context.Warga.Remove(warga);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool WargaExists(int id)
    {
        return _context.Warga.Any(e => e.IdWarga == id);
    }
}