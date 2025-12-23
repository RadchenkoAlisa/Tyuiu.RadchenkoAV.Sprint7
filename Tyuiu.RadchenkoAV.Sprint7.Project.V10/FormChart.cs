using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Tyuiu.RadchenkoAV.Sprint7.ProjectV10.Lib;

namespace Tyuiu.RadchenkoAV.Sprint7.ProjectV10
{
    public partial class FormChart_RAV : Form
    {
        private List<DataService.Order> orders;

        public FormChart_RAV(List<DataService.Order> orders)
        {
            this.orders = orders;
            InitializeComponent();
            this.Text = "График стоимости заказов";
            this.Size = new Size(900, 650);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.White;
        }

        private void InitializeComponent()
        {
            this.Paint += new PaintEventHandler(FormChart_Paint_RAV);

            // Кнопка закрытия
            Button buttonClose_RAV = new Button();
            buttonClose_RAV.Text = "✖ Закрыть";
            buttonClose_RAV.Location = new Point(380, 580);
            buttonClose_RAV.Size = new Size(120, 35);
            buttonClose_RAV.Font = new Font("Arial", 10);
            buttonClose_RAV.BackColor = Color.FromArgb(217, 83, 79);
            buttonClose_RAV.ForeColor = Color.White;
            buttonClose_RAV.FlatStyle = FlatStyle.Flat;
            buttonClose_RAV.Click += (s, e) => this.Close();

            this.Controls.Add(buttonClose_RAV);
        }

        private void FormChart_Paint_RAV(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(Color.White);

            if (orders.Count == 0)
            {
                g.DrawString("Нет данных для отображения графика",
                    new Font("Arial", 14, FontStyle.Bold), Brushes.Black, 250, 250);
                return;
            }

            // Упорядочим заказы по номеру
            var sortedOrders = orders.OrderBy(o => o.OrderNumber).ToList();

            Pen pen = new Pen(Color.Blue, 2);
            Brush barBrush = new SolidBrush(Color.FromArgb(65, 105, 225));
            Font font = new Font("Arial", 9);
            Font boldFont = new Font("Arial", 10, FontStyle.Bold);

            int leftMargin = 100;
            int rightMargin = 100;
            int topMargin = 100;
            int bottomMargin = 150;

            int chartWidth = this.ClientSize.Width - leftMargin - rightMargin;
            int chartHeight = this.ClientSize.Height - topMargin - bottomMargin;

            // Находим максимальную стоимость
            decimal maxCost = sortedOrders.Max(o => o.OrderCost);
            if (maxCost == 0) maxCost = 1;

            // Рисуем оси
            g.DrawLine(Pens.Black, leftMargin, topMargin + chartHeight, leftMargin, topMargin);
            g.DrawLine(Pens.Black, leftMargin, topMargin + chartHeight, leftMargin + chartWidth, topMargin + chartHeight);

            // Добавляем стрелки на осях
            // Стрелка на оси Y
            g.DrawLine(Pens.Black, leftMargin, topMargin, leftMargin - 5, topMargin + 10);
            g.DrawLine(Pens.Black, leftMargin, topMargin, leftMargin + 5, topMargin + 10);
            // Стрелка на оси X
            g.DrawLine(Pens.Black, leftMargin + chartWidth, topMargin + chartHeight,
                leftMargin + chartWidth - 10, topMargin + chartHeight - 5);
            g.DrawLine(Pens.Black, leftMargin + chartWidth, topMargin + chartHeight,
                leftMargin + chartWidth - 10, topMargin + chartHeight + 5);

            // Подписи к осям
            g.DrawString("Стоимость (руб.)", boldFont, Brushes.Black,
                leftMargin - 90, topMargin - 30);
            g.DrawString("Номер заказа", boldFont, Brushes.Black,
                leftMargin + chartWidth / 2 - 50, topMargin + chartHeight + 30);

            // Сетка и подписи на оси Y
            for (int i = 0; i <= 5; i++)
            {
                int yPos = topMargin + chartHeight - (i * chartHeight / 5);
                decimal value = maxCost * i / 5;

                // Линия сетки
                g.DrawLine(new Pen(Color.LightGray, 1), leftMargin, yPos, leftMargin + chartWidth, yPos);

                // Подпись значения
                string label = value.ToString("C0");
                SizeF textSize = g.MeasureString(label, font);
                g.DrawString(label, font, Brushes.Black,
                    leftMargin - textSize.Width - 10, yPos - textSize.Height / 2);
            }

            // Рисуем столбцы
            int displayCount = Math.Min(sortedOrders.Count, 15);
            int barWidth = chartWidth / (displayCount + 2); // Уменьшаем ширину столбцов для лучшего отображения

            for (int i = 0; i < displayCount; i++)
            {
                var order = sortedOrders[i];

                int barHeight = (int)(chartHeight * (double)(order.OrderCost / maxCost));
                int x = leftMargin + (i + 1) * barWidth;
                int y = topMargin + chartHeight - barHeight;

                // Столбец
                g.FillRectangle(barBrush, x, y, barWidth - 10, barHeight);
                g.DrawRectangle(Pens.Black, x, y, barWidth - 10, barHeight);

                // Подпись номера заказа внизу
                string orderLabel = $"№{order.OrderNumber}";
                SizeF textSize = g.MeasureString(orderLabel, font);
                g.DrawString(orderLabel, font, Brushes.Black,
                    x + (barWidth - 10 - textSize.Width) / 2,
                    topMargin + chartHeight + 10);

                // Стоимость над столбцом (или внутри, если столбец высокий)
                string costLabel = order.OrderCost.ToString("C0");
                textSize = g.MeasureString(costLabel, font);

                if (barHeight > 25)
                {
                    // Внутри столбца, если он достаточно высокий
                    g.DrawString(costLabel, boldFont, Brushes.White,
                        x + (barWidth - 10 - textSize.Width) / 2,
                        y + 5);
                }
                else
                {
                    // Над столбцом, если он слишком низкий
                    g.DrawString(costLabel, font, Brushes.Black,
                        x + (barWidth - 10 - textSize.Width) / 2,
                        y - textSize.Height - 2);
                }
            }

            // Заголовок
            string title = "📊 ГИСТОГРАММА СТОИМОСТИ ЗАКАЗОВ";
            Font titleFont = new Font("Arial", 16, FontStyle.Bold);
            SizeF titleSize = g.MeasureString(title, titleFont);
            g.DrawString(title, titleFont, Brushes.Black,
                (this.ClientSize.Width - titleSize.Width) / 2, 30);

            // Статистика внизу
            string stats = $"📈 Всего заказов: {sortedOrders.Count} | " +
                          $"💰 Макс. стоимость: {maxCost:C} | " +
                          $"📊 Средняя: {sortedOrders.Average(o => o.OrderCost):C}";
            g.DrawString(stats, boldFont, Brushes.Black,
                leftMargin, topMargin + chartHeight + 60);

            // Легенда
            int legendX = leftMargin + chartWidth - 150;
            int legendY = topMargin - 30;

            g.FillRectangle(barBrush, legendX, legendY, 20, 15);
            g.DrawRectangle(Pens.Black, legendX, legendY, 20, 15);
            g.DrawString("Стоимость заказа", font, Brushes.Black, legendX + 25, legendY);

            // Освобождаем ресурсы
            pen.Dispose();
            barBrush.Dispose();
            font.Dispose();
            boldFont.Dispose();
            titleFont.Dispose();
        }
    }
}