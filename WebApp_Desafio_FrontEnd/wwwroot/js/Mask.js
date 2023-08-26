// custom-functions.js
function validarCPF(cpf) {
    cpf = cpf.replace(/[^\d]+/g, ''); // Remove caracteres não numéricos
    if (cpf.length !== 11) return false; // CPF deve ter 11 dígitos

    // Verificação de CPFs com todos os dígitos iguais
    if (/^(\d)\1+$/.test(cpf)) return false;

    // Cálculo dos dígitos verificadores
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
