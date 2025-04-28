namespace API.Utility
{
    public static class Validacao
    {
        public static bool ValidarEmail(string email)
        {
            if (email.Trim().EndsWith("."))
                return false;

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public static bool ValidarCpf(string cpf)
        {
            try
            {
                cpf = cpf.Replace(".", "").Replace("-", "").Replace("/", "").Trim();
                int num1, num2, num3, num4, num5, num6, num7, num8, num9, num10, num11;
                int soma1, soma2;
                int resto1, resto2;

                num1 = int.Parse(cpf.Substring(0, 1));
                num2 = int.Parse(cpf.Substring(1, 1));
                num3 = int.Parse(cpf.Substring(2, 1));
                num4 = int.Parse(cpf.Substring(3, 1));
                num5 = int.Parse(cpf.Substring(4, 1));
                num6 = int.Parse(cpf.Substring(5, 1));
                num7 = int.Parse(cpf.Substring(6, 1));
                num8 = int.Parse(cpf.Substring(7, 1));
                num9 = int.Parse(cpf.Substring(8, 1));
                num10 = int.Parse(cpf.Substring(9, 1));
                num11 = int.Parse(cpf.Substring(10, 1));

                if ((num1 == num2) && (num2 == num3) && (num3 == num4) && (num4 == num5) && (num5 == num6) && (num6 == num7) && (num7 == num8) && (num8 == num9) && (num9 == num10) && (num10 == num11))
                    return false;
                else
                {
                    soma1 = num1 * 10 + num2 * 9 + num3 * 8 + num4 * 7 + num5 * 6 + num6 * 5 + num7 * 4 + num8 * 3 + num9 * 2;
                    resto1 = (soma1 * 10) % 11;
                    if (resto1 == 10)
                        resto1 = 0;

                    soma2 = num1 * 11 + num2 * 10 + num3 * 9 + num4 * 8 + num5 * 7 + num6 * 6 + num7 * 5 + num8 * 4 + num9 * 3 + num10 * 2;
                    resto2 = (soma2 * 10) % 11;

                    if (resto2 == 10)
                        resto2 = 0;

                    if (resto1 == num10 && resto2 == num11)
                        return true;
                    else
                        return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool ValidarCnpj(string cnpj)
        {
            try
            {
                cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "").Trim();
                int[] multiplicador1 = new int[12] { 6, 7, 8, 9, 2, 3, 4, 5, 6, 7, 8, 9 };
                int[] multiplicador2 = new int[13] { 5, 6, 7, 8, 9, 2, 3, 4, 5, 6, 7, 8, 9 };
                int soma, soma2;
                int resto, resto2;
                string digito;
                string tempCnpj;

                var cnpjSplit = new short[14];

                cnpjSplit[0] = short.Parse(cnpj.Substring(0, 1));
                cnpjSplit[1] = short.Parse(cnpj.Substring(1, 1));
                cnpjSplit[2] = short.Parse(cnpj.Substring(2, 1));
                cnpjSplit[3] = short.Parse(cnpj.Substring(3, 1));
                cnpjSplit[4] = short.Parse(cnpj.Substring(4, 1));
                cnpjSplit[5] = short.Parse(cnpj.Substring(5, 1));
                cnpjSplit[6] = short.Parse(cnpj.Substring(6, 1));
                cnpjSplit[7] = short.Parse(cnpj.Substring(7, 1));
                cnpjSplit[8] = short.Parse(cnpj.Substring(8, 1));
                cnpjSplit[9] = short.Parse(cnpj.Substring(9, 1));
                cnpjSplit[10] = short.Parse(cnpj.Substring(10, 1));
                cnpjSplit[11] = short.Parse(cnpj.Substring(11, 1));
                cnpjSplit[12] = short.Parse(cnpj.Substring(12, 1));
                cnpjSplit[13] = short.Parse(cnpj.Substring(13, 1));

                if ((cnpjSplit[0] == cnpjSplit[1]) && (cnpjSplit[2] == cnpjSplit[3]) && (cnpjSplit[4] == cnpjSplit[5])
                     && (cnpjSplit[6] == cnpjSplit[7]) && (cnpjSplit[8] == cnpjSplit[9]) && (cnpjSplit[10] == cnpjSplit[11])
                      && (cnpjSplit[12] == cnpjSplit[13]))
                {
                    return false;
                }

                tempCnpj = cnpj.Substring(0, 12);
                soma = 0;
                for (int i = 0; i < 12; i++)
                    soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];
                resto = (soma % 11);

                digito = resto.ToString();
                tempCnpj += digito;
                soma2 = 0;
                for (int i = 0; i < 13; i++)
                    soma2 += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];
                resto2 = (soma2 % 11);

                digito += resto2.ToString();

                return cnpj.EndsWith(digito);

            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
