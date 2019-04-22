using pMedicoCompleto.DAO;
using pMedicoCompleto.Utils;
using pMedicoCompleto.ViewModels;
using System.Linq;
using System.Security.Claims;
using System.Web.Mvc;

namespace pMedicoCompleto.Controllers
{
    public class PerfilController : Controller
    {
        private EFContext db = new EFContext();
        // GET: Perfil
        [Authorize]
        public ActionResult AlterarSenha()
        {
            return View();
            
        }

        [Authorize]
        [HttpPost]
        public ActionResult AlterarSenha(AlterarSenhaViewModels viewmodel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var identity = User.Identity as ClaimsIdentity;
            var login = identity.Claims.FirstOrDefault(c => c.Type == "Login").Value;
            var usuario = db.Usuarios.FirstOrDefault(u => u.Login == login);

            if (Hash.gerarHash(viewmodel.senhaAtual) != usuario.Senha)
            {
                ModelState.AddModelError("senhaAtual", "Senha Incorreta");
                return View();
            }
            usuario.Senha = Hash.gerarHash(viewmodel.senhaNova);
            db.Entry(usuario).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index", "Painel");

        }
    }
}