using MortgageCalc.Models;

namespace MortgageCalc.Helpers {
    public class LoanHelper {
        public Loan GetPayments(Loan loan) {
            // calculate monthly payment
            loan.Payment = CalcPayment(loan.Amount, loan.Rate, loan.Term);

            var balance = loan.Amount;
            var totalInterest = 0.0m;
            var monthlyInterest = 0.0m;
            var monthlyPrincipal = 0.0m;
            var monthlyRate = CalcMonthlyRate(loan.Rate);

            // loop over each month until I reach the term of the loan
            for (int month = 1; month < loan.Term; month++) {
                monthlyInterest = CalcMonthlyInterest(balance, monthlyRate);
                totalInterest += monthlyInterest;
                monthlyPrincipal = loan.Payment - monthlyInterest;
                balance -= monthlyPrincipal;

                LoanPayment loanPayment = new();

                loanPayment.Month = month;
                loanPayment.Payment = loan.Payment;
                loanPayment.MonthlyPrincipal = monthlyPrincipal;
                loanPayment.MonthlyInterest = monthlyInterest;
                loanPayment.TotalInterest = totalInterest;
                loanPayment.Balance = balance;

                loan.Payments.Add(loanPayment); // push payments into the loan model
            }

            loan.TotalInterest = totalInterest;
            loan.TotalCost = loan.Amount + totalInterest;

            return loan; // return the loan to the view
        }

        private decimal CalcPayment(decimal amount, decimal rate, int term) {
            // conversions
            var monthlyRate = CalcMonthlyRate(rate); // calculate monthly rate from annual input
            var rateD = Convert.ToDouble(rate);
            var amountD = Convert.ToDouble(amount);

            var paymentD = (amountD * rateD) / (1 - Math.Pow(1 + rateD, -term)); // formula

            return Convert.ToDecimal(paymentD);
        }

        private decimal CalcMonthlyRate(decimal rate) {
            return rate / 1200;
        }

        private decimal CalcMonthlyInterest(decimal balance, decimal monthlyRate) {
            return balance * monthlyRate;
        }
    }
}
