function validaCPF(cpf) {
    cpf = cpf.replace(/[^\d]+/g, '');

    if (cpf == '' || cpf.length != 11 ||
        cpf == '01234567890' ||
        cpf == "00000000000" ||
        cpf == "11111111111" ||
        cpf == "22222222222" ||
        cpf == "33333333333" ||
        cpf == "44444444444" ||
        cpf == "55555555555" ||
        cpf == "66666666666" ||
        cpf == "77777777777" ||
        cpf == "88888888888" ||
        cpf == "99999999999")
        return false;

    var i;
    var l = '';
    for (i = 0; i < cpf.length; i++) if (!isNaN(cpf.charAt(i))) l += cpf.charAt(i);
    cpf = l;
    if (cpf.length != 11) return false;
    var c = cpf.substr(0, 9);
    var dv = cpf.substr(9, 2);
    var d1 = 0;
    for (i = 0; i < 9; i++) d1 += c.charAt(i) * (10 - i);
    if (d1 == 0) return false;
    d1 = 11 - (d1 % 11);
    if (d1 > 9) d1 = 0;
    if (dv.charAt(0) != d1) return false;
    d1 *= 2;
    for (i = 0; i < 9; i++) d1 += c.charAt(i) * (11 - i)
    d1 = 11 - (d1 % 11);
    if (d1 > 9) d1 = 0;
    if (dv.charAt(1) != d1) return false;

    return true;
}