// custom-functions.js
function validarCPF(cpf) {
    cpf = cpf.replace(/[^\d]+/g, ''); // Remove caracteres n�o num�ricos
    if (cpf.length !== 11) return false; // CPF deve ter 11 d�gitos

    // Verifica��o de CPFs com todos os d�gitos iguais
    if (/^(\d)\1+$/.test(cpf)) return false;

    // C�lculo dos d�gitos verificadores
    let sum = 0;
    let mod;

    for (let i = 1; i <= 9; i++) {
        sum += parseInt(cpf.substring(i - 1, i)) * (11 - i);
    }

    mod = (sum * 10) % 11;
    if (mod === 10 || mod === 11) mod = 0;
    if (mod !== parseInt(cpf.substring(9, 10))) return false;

    sum = 0;
    for (let i = 1; i <= 10; i++) {
        sum += parseInt(cpf.substring(i - 1, i)) * (12 - i);
    }

    mod = (sum * 10) % 11;
    if (mod === 10 || mod === 11) mod = 0;
    if (mod !== parseInt(cpf.substring(10, 11))) return false;

    return true;
}
