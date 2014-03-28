using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BPLLCWEB.Classes
{
    public class Reusables
    {
        public static string GetPassword()
        {
            string theAlphabet = null;
            string retValue = null;
            int iSize = 0;
            int iRandomNumber = 0;

            // we will NEVER have ZERO, Capital OH, number one, lowercase el or uppercase que in the password
            theAlphabet = "23456EFGHjkmnop435635684679qrstu789ABCmonkey76867896789heesefoodDvwxyzJKLM79897NPabcdefghiRSTU7asdf823r9nvV6789WXYZ2345";
            iSize = theAlphabet.Length;
            Random random = new Random();

            for (int theCount = 0; theCount <= 9; theCount++)
            {
                iRandomNumber = random.Next(1, iSize);
                retValue += theAlphabet.Substring(iRandomNumber, 1);
            }
            return retValue;
        }

        public static string ContactSubject(string contactcode)
        {
            string subject = "";
            switch (contactcode)
            {
                case "crel":
                    subject = "Commercial Real Estate Loans - Lending";
                    break;

                case "cres":
                    subject = "Commercial Real Estate Loans - Servicing";
                    break;

                case "sbal":
                    subject = "SBA Guaranteed Loans - Lending";
                    break;

                case "sbas":
                    subject = "SBA Guaranteed Loans - Servicing";
                    break;

                case "osbal":
                    subject = "Other Small Business Loans and Lines of Credit - Lending";
                    break;

                case "osbas":
                    subject = "Other Small Business Loans and Lines of Credit - Servicing";
                    break;

                case "r":
                    subject = "Small Business Loan Credit Bureau Reporting";
                    break;

                case "ir":
                    subject = "Investor Relations";
                    break;

                case "bpc":
                    subject = "Additional Info or Request";
                    break;

                case "q":
                    subject = "Web Access Questions";
                    break;
            }

            return subject;
        }

        public static string ContactRecipient(string contactcode)
        {
            string email = "";

            switch (contactcode)
            {
                case "crel":
                    email = "Lending@BusinessPartnersLLC.com";
                    break;

                case "cres":
                    email = "LoanServicing@BusinessPartnersLLC.com";
                    break;

                case "sbal":
                    email = "SBALending@BusinessPartnersLLC.com";
                    break;

                case "sbas":
                    email = "SBAServicing@BusinessPartnersLLC.com";
                    break;

                case "osbal":
                    email = "SmallBusinessLending@BusinessPartnersLLC.com";
                    break;

                case "osbas":
                    email = "SmallBusinessServicing@BusinessPartnersLLC.com";
                    break;

                case "r":
                    email = "SBSBureau@BusinessPartnersLLC.com";
                    break;

                case "ir":
                    email = "InvestorRelations@BusinessPartnersLLC.com";
                    break;

                case "bpc":
                    email = "ben.wilson@BusinessPartnersLLC.com,scott.orsatti@BusinessPartnersLLC.com";
                    break;

                case "q":
                    email = "WebAccess@BusinessPartnersLLC.com";
                    break;
            }

            return email;
        }
    }
}