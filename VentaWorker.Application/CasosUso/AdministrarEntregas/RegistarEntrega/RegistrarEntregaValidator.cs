using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntregaWorker.Application.CasosUso.AdministrarEntregas.RegistarEntrega
{
    public class RegistrarEntregaValidator : AbstractValidator<RegistrarEntregaRequest>
    {
        public RegistrarEntregaValidator()
        {
            //RuleFor(item => item.Nombre).NotEmpty().WithMessage("Debe especificar un nombre");
            //RuleFor(item => item.Stock).NotEmpty().WithMessage("Debe especificar un Stock");
            //RuleFor(item => item.PrecioUnitario).NotEmpty().WithMessage("Debe especificar un precio unitario");
        }
    }
}
