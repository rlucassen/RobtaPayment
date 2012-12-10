using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobtaPayment.Model.Helpers
{
    using Entities;
    using RobtaPayment.Model.Enums;

    public static class CsvHelper
    {
        private const string ActivityFormat = @"{1}{0}{2}{0}{3}{0}{4}{0}{5}";
        private const string LockerFormat = @"{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}{6}";
        private const string ContributionFormat = @"{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}{6}{0}{7}";
        private const string ExamFormat = @"{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}{6}{0}{7}{0}{8}";

        public static string GenerateCsvForExamEnrolments(Exam exam)
        {
            var sb = new StringBuilder();
            sb.Append(GenerateCsvHeaderForExamEnrolments());
            sb.Append(LineEnding);
            foreach (ExamEnrolment enrolment in exam.Enrolments)
            {
                sb.Append(GenerateCsvLineForExamEnrolment(enrolment));
                sb.Append(LineEnding);
            }
            return sb.ToString();
        }

        private static string GenerateCsvHeaderForExamEnrolments()
        {
            return String.Format(ExamFormat, Separator, "Voornaam", "Tussenvoegsel", "Achternaam", "Email", "Toets", "Opleiding", "Paid", "Betaaldatum");
        }

        private static string GenerateCsvLineForExamEnrolment(ExamEnrolment enrolment)
        {
            return String.Format(ExamFormat,
                                 Separator,
                                 ToSafe(enrolment.Name),
                                 ToSafe(enrolment.Preposition),
                                 ToSafe(enrolment.Lastname),
                                 ToSafe(enrolment.Email),
                                 ToSafe(enrolment.Exam.Name),
                                 ToSafe(enrolment.Education == null ? "" : enrolment.Education.Name),
                                 ToSafe(enrolment.Paid.ToString()),
                                 ToSafe(enrolment.EnrolmentDate.ToString("yyyy-MM-dd")));
        }

        public static string GenerateCsvForContributionsEnrolments(Contribution contribution)
        {
            var sb = new StringBuilder();
            sb.Append(GenerateCsvHeaderForContributionEnrolments());
            sb.Append(LineEnding);
            foreach (ContributionEnrolment enrolment in contribution.Enrolments)
            {
                sb.Append(GenerateCsvLineForContributionEnrolment(enrolment));
                sb.Append(LineEnding);
            }
            return sb.ToString();
        }

        private static string GenerateCsvHeaderForContributionEnrolments()
        {
            return String.Format(ContributionFormat, Separator, "StudentNumber", "Name", "Email", "Transactiondate", "Paid", "Amount", "Education");
        }

        private static string GenerateCsvLineForContributionEnrolment(ContributionEnrolment enrolment)
        {
            return String.Format(ContributionFormat,
                                 Separator,
                                 ToSafe(enrolment.StudentNumber),
                                 ToSafe(enrolment.Name),
                                 ToSafe(enrolment.Email),
                                 ToSafe(enrolment.EnrolmentDate.ToString("yyyy-MM-dd")),
                                 ToSafe(enrolment.Paid.ToString()), 
                                 ToSafe(enrolment.Transaction.Amount.ToString("c")),
                                 ToSafe(ContributionTypeTranslator.GetString(enrolment.Contribution.ContributionType)));
        }
       
        public static string GenerateCsvForActivityEnrolments(Activity activity)
        {
            var sb = new StringBuilder();
            foreach(ActivityEnrolment enrolment in activity.Enrolments)
            {
                sb.Append(GenerateCsvLineForActivityEnrolment(enrolment));
                sb.Append(LineEnding);
            }
            return sb.ToString();
        }

        private static string GenerateCsvLineForActivityEnrolment(ActivityEnrolment enrolment)
        {
            return String.Format(ActivityFormat, 
                                 Separator, 
                                 ToSafe(enrolment.StudentNumber), 
                                 ToSafe(enrolment.Name), 
                                 ToSafe(enrolment.Email),
                                 ToSafe(enrolment.EnrolmentDate.ToString("yyyy-MM-dd")), 
                                 ToSafe(enrolment.Paid.ToString()));
        }

        public static string GenerateCsvForLockerEnrolments(List<LockerEnrolment> lockerEnrolments)
        {
            var sb = new StringBuilder();
            sb.Append(GenerateCsvHeaderForLockerEnrolments());
            sb.Append(LineEnding);
            foreach (LockerEnrolment enrolment in lockerEnrolments)
            {
                sb.Append(GenerateCsvLineForLockerEnrolment(enrolment));
                sb.Append(LineEnding);
            }
            return sb.ToString();
        }

        private static string GenerateCsvLineForLockerEnrolment(LockerEnrolment enrolment)
        {
            return String.Format(LockerFormat,
                                 Separator,
                                 ToSafe(enrolment.StudentNumber),
                                 ToSafe(enrolment.Name),
                                 ToSafe(enrolment.Email),
                                 ToSafe(enrolment.SchoolYear.Name),
                                 ToSafe(enrolment.Paid.ToString()),
                                 String.Format("http://reserveringen.RobtaPayment.nl/verleng/{0}", enrolment.Locker.Id));
        }

        private static string GenerateCsvHeaderForLockerEnrolments()
        {
            return String.Format(LockerFormat, Separator, "StudentNumber", "Name", "Email", "Schoolyear", "Paid", "Verleng URL");
        }

        public static Char LineEnding
        {
            get { return '\x000A'; }
        }

        public static Char Separator
        {
            get { return '\x003B'; }
        }

        public static string ToSafe(string text)
        {
            try
            {
                return text.Replace(Separator, '_').Replace(LineEnding, ' ');
            }
            catch (Exception)
            {
                return "";
            }
            
        }
    }
}
