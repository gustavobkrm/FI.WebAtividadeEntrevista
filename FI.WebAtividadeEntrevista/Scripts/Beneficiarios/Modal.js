function AbrirModal() { 
    $("#CPFBeneficiario").inputmask('999.999.999-99')
}

function IncluirBeneficiario() {
    var cpf = $('#CPFBeneficiario').val();
    var nome = $('#NomeBeneficiario').val();

    if (cpf && nome) {
        $('#beneficiariosGrid').append(
            `<div class="row item-modal" style="margin:0 0 0 0">
                <div class="col-md-4">
                  <div class="form-group">
                    <input type="text" class="form-control cpf" value="${cpf}" readonly>
                  </div>
                </div>
                <div class="col-md-4">
                  <div class="form-group">
                    <input type="text" class="form-control nome" value="${nome}" readonly>
                  </div>
                </div>
                <div class="col-md-4">
                  <div class="form-group">
                    <button class="btn btn-success btn-sm salvar" style="display: none;">Salvar</button>
                    <button class="btn btn-primary btn-sm alterar">Alterar</button>
                    <button class="btn btn-primary btn-sm excluir">Excluir</button>
                  </div>
                </div>
            </div>
            `
        )
        $('#CPFBeneficiario').val('');
        $('#NomeBeneficiario').val('');

        AtualizaListaBeneficiarios();
    }
}

function AtualizaListaBeneficiarios() {
    var beneficiarios = [];

    $('#beneficiariosGrid .item-modal').each(function () {
        var cpf = $(this).find('.cpf').val();
        var nome = $(this).find('.nome').val();

        beneficiarios.push({ Id: 0, Nome: nome, CPF: cpf, IdCliente: 0 });
    });

    $('#Beneficiarios').val(JSON.stringify(beneficiarios));
}

$(document).ready(() => {
    $('#beneficiariosGrid').on('click', '.excluir', function () {
        $(this).closest('.item-modal').remove();
    });
    $('#beneficiariosGrid').on('click', '.alterar', function () {
        const itemModal = $(this).closest('.item-modal');

        itemModal.find('.cpf, .nome').prop('readonly', false);
        itemModal.find('.alterar').hide();
        itemModal.find('.salvar').show();
    });

    $('#beneficiariosGrid').on('click', '.salvar', function () {
        const itemModal = $(this).closest('.item-modal');

        itemModal.find('.cpf, .nome').prop('readonly', true);

        itemModal.find('.alterar').show();
        itemModal.find('.salvar').hide();

    });

});
