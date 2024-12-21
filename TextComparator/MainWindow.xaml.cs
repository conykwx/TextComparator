using System;
using System.Windows;
using System.Windows.Media;

namespace TextComparator
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            string text1 = TextArea1.Text;
            string text2 = TextArea2.Text;

            int distance = CalculateLevenshteinDistance(text1, text2);
            double similarity = text1.Length > 0 || text2.Length > 0
                ? 100.0 * (1 - (double)distance / Math.Max(text1.Length, text2.Length))
                : 100;

            UpdateSimilarityDisplay(similarity);
        }

        private int CalculateLevenshteinDistance(string s1, string s2)
        {
            int[,] dp = new int[s1.Length + 1, s2.Length + 1];

            for (int i = 0; i <= s1.Length; i++) dp[i, 0] = i;
            for (int j = 0; j <= s2.Length; j++) dp[0, j] = j;

            for (int i = 1; i <= s1.Length; i++)
            {
                for (int j = 1; j <= s2.Length; j++)
                {
                    int cost = s1[i - 1] == s2[j - 1] ? 0 : 1;
                    dp[i, j] = Math.Min(
                        Math.Min(dp[i - 1, j] + 1, dp[i, j - 1] + 1),
                        dp[i - 1, j - 1] + cost
                    );
                }
            }

            return dp[s1.Length, s2.Length];
        }

        private void UpdateSimilarityDisplay(double similarity)
        {
            SimilarityText.Text = $"Similarity: {similarity:F2}%";

            if (similarity < 33)
                SimilarityText.Foreground = Brushes.Red;
            else if (similarity < 66)
                SimilarityText.Foreground = Brushes.Orange;
            else
                SimilarityText.Foreground = Brushes.Green;
        }
    }
}
