using DomainLayer.Models;

namespace PracticasCSAvanzado.Custom
{
    public interface IUtility
    {
        string EncriptarSHA256(string input);
        string GenerarJWT(Usuario usuario);
    }
}

