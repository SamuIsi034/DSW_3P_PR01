using DSW_3P_PR01.Models;
using DSW_3P_PR01.Repositories;

namespace DSW_3P_PR01.Services;

public class MisionService : IMisionService
{
    private readonly IMisionRepository _repository;
    private static readonly string[] DificultadesValidas = { "Fácil", "Media", "Difícil" };

    public MisionService(IMisionRepository repository)
    {
        _repository = repository;
    }

    public List<Mision> ObtenerTodas()
    {
        return _repository.ObtenerTodas();
    }

    public Mision? ObtenerPorId(int id)
    {
        return _repository.ObtenerPorId(id);
    }

    public Mision Agregar(Mision nuevaMision)
    {
        ValidarMision(nuevaMision);

        // Regla de negocio: Todas las misiones nuevas deberán iniciar como no completadas.
        nuevaMision.Completada = false;

        return _repository.Agregar(nuevaMision);
    }

    public bool Actualizar(int id, Mision mision)
    {
        // Regla de negocio: No se deberán permitir modificaciones de misiones inexistentes.
        var existente = _repository.ObtenerPorId(id);
        if (existente == null)
        {
            return false;
        }

        ValidarMision(mision);

        mision.Id = id;
        return _repository.Actualizar(id, mision);
    }

    public bool Eliminar(int id)
    {
        // Regla de negocio: No se deberán permitir eliminaciones de misiones inexistentes.
        var existente = _repository.ObtenerPorId(id);
        if (existente == null)
        {
            return false;
        }

        return _repository.Eliminar(id);
    }

    private void ValidarMision(Mision mision)
    {
        if (string.IsNullOrWhiteSpace(mision.Nombre))
        {
            throw new ArgumentException("El nombre de la misión es obligatorio.");
        }

        if (!DificultadesValidas.Contains(mision.Dificultad))
        {
            throw new ArgumentException("La dificultad únicamente podrá ser Fácil, Media o Difícil.");
        }

        if (mision.RecompensaXP < 0)
        {
            throw new ArgumentException("La recompensa XP no puede ser negativa.");
        }
    }
}
