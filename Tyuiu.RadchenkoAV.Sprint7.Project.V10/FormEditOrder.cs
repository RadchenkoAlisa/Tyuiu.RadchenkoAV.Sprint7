using System;
using System.Drawing;
using System.Windows.Forms;
using Tyuiu.RadchenkoAV.Sprint7.ProjectV10.Lib;

namespace Tyuiu.RadchenkoAV.Sprint7.ProjectV10
{
    public partial class FormEditOrder_RAV : Form
    {
        private DataService.Order order;
        private bool isMaximized = false;
        private Size originalSize;
        private Point originalLocation;

        public FormEditOrder_RAV()
        {
            InitializeComponent();
            order = new DataService.Order
            {
                OrderNumber = GetNextOrderNumber(),
                ExecutionDate = DateTime.Now.AddDays(7),
                ProductName = "",
                ClientLastName = "",
                ClientFirstName = "",
                ClientMiddleName = "",
                AccountNumber = "",
                Address = "",
                Phone = "",
                OrderCost = 0,
                ProductPrice = 0,
                Quantity = 1
            };
            this.Text = "Добавить новый заказ";

            // Сохраняем оригинальный размер и позицию
            originalSize = this.Size;
            originalLocation = this.Location;

            // Добавляем обработчик изменения размера
            this.Resize += FormEditOrder_RAV_Resize;
        }

        public FormEditOrder_RAV(DataService.Order existingOrder)
        {
            InitializeComponent();
            order = existingOrder;
            LoadOrderData();
            this.Text = "Редактировать заказ";

            originalSize = this.Size;
            originalLocation = this.Location;
            this.Resize += FormEditOrder_RAV_Resize;
        }

        private int GetNextOrderNumber()
        {
            return new Random().Next(1000, 9999);
        }

        private void InitializeComponent()
        {
            this.MinimumSize = new Size(650, 600);
            this.Size = new Size(750, 800);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.MaximizeBox = true;
            this.MinimizeBox = true;
            this.BackColor = Color.FromArgb(240, 240, 240);

            // Панель заголовка с кнопками управления
            Panel panelHeader_RAV = new Panel();
            panelHeader_RAV.Dock = DockStyle.Top;
            panelHeader_RAV.Height = 50;
            panelHeader_RAV.BackColor = Color.FromArgb(51, 122, 183);

            // Заголовок формы
            Label labelHeader_RAV = new Label();
            labelHeader_RAV.Text = "📝 Форма заказа";
            labelHeader_RAV.Dock = DockStyle.Fill;
            labelHeader_RAV.TextAlign = ContentAlignment.MiddleCenter;
            labelHeader_RAV.Font = new Font("Arial", 14, FontStyle.Bold);
            labelHeader_RAV.ForeColor = Color.White;

            // Панель кнопок управления окном
            Panel panelWindowControls_RAV = new Panel();
            panelWindowControls_RAV.Dock = DockStyle.Right;
            panelWindowControls_RAV.Width = 100;
            panelWindowControls_RAV.BackColor = Color.Transparent;

            // Кнопка полноэкранного режима
            Button buttonFullScreen_RAV = new Button();
            buttonFullScreen_RAV.Text = "⛶";
            buttonFullScreen_RAV.Size = new Size(30, 30);
            buttonFullScreen_RAV.Location = new Point(10, 10);
            buttonFullScreen_RAV.Font = new Font("Arial", 12);
            buttonFullScreen_RAV.BackColor = Color.Transparent;
            buttonFullScreen_RAV.ForeColor = Color.White;
            buttonFullScreen_RAV.FlatStyle = FlatStyle.Flat;
            buttonFullScreen_RAV.FlatAppearance.BorderSize = 0;
            buttonFullScreen_RAV.Click += ButtonFullScreen_Click_RAV;
            buttonFullScreen_RAV.MouseEnter += (s, e) => buttonFullScreen_RAV.BackColor = Color.FromArgb(100, 255, 255, 255);
            buttonFullScreen_RAV.MouseLeave += (s, e) => buttonFullScreen_RAV.BackColor = Color.Transparent;

            panelWindowControls_RAV.Controls.Add(buttonFullScreen_RAV);
            panelHeader_RAV.Controls.Add(panelWindowControls_RAV);
            panelHeader_RAV.Controls.Add(labelHeader_RAV);

            // Основной контейнер с скроллингом
            Panel panelMainContainer_RAV = new Panel();
            panelMainContainer_RAV.Dock = DockStyle.Fill;
            panelMainContainer_RAV.BackColor = Color.White;

            // Панель с полями ввода (будет менять размер)
            panelMain_RAV = new Panel();
            panelMain_RAV.Location = new Point(0, 0);
            panelMain_RAV.Size = new Size(730, 650);
            panelMain_RAV.AutoScroll = true;
            panelMain_RAV.BackColor = Color.White;

            InitializeFormFields();

            panelMainContainer_RAV.Controls.Add(panelMain_RAV);

            // Панель кнопок действий
            Panel panelButtons_RAV = new Panel();
            panelButtons_RAV.Dock = DockStyle.Bottom;
            panelButtons_RAV.Height = 80;
            panelButtons_RAV.BackColor = Color.FromArgb(248, 249, 250);
            panelButtons_RAV.Padding = new Padding(10);

            Button buttonSave_RAV = new Button();
            buttonSave_RAV.Text = "💾 Сохранить";
            buttonSave_RAV.Size = new Size(160, 45);
            buttonSave_RAV.Font = new Font("Arial", 10, FontStyle.Bold);
            buttonSave_RAV.BackColor = Color.FromArgb(92, 184, 92);
            buttonSave_RAV.ForeColor = Color.White;
            buttonSave_RAV.FlatStyle = FlatStyle.Flat;
            buttonSave_RAV.Location = new Point(200, 15);
            buttonSave_RAV.Click += ButtonSave_Click_RAV;

            Button buttonCancel_RAV = new Button();
            buttonCancel_RAV.Text = "❌ Отмена";
            buttonCancel_RAV.Size = new Size(160, 45);
            buttonCancel_RAV.Font = new Font("Arial", 10, FontStyle.Bold);
            buttonCancel_RAV.BackColor = Color.FromArgb(217, 83, 79);
            buttonCancel_RAV.ForeColor = Color.White;
            buttonCancel_RAV.FlatStyle = FlatStyle.Flat;
            buttonCancel_RAV.Location = new Point(380, 15);
            buttonCancel_RAV.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };

            panelButtons_RAV.Controls.AddRange(new Control[] { buttonSave_RAV, buttonCancel_RAV });

            this.Controls.AddRange(new Control[] { panelHeader_RAV, panelMainContainer_RAV, panelButtons_RAV });

            // Подписываемся на события изменения цены и количества
            textBoxProductPrice_RAV.TextChanged += CalculateTotalCost_RAV;
            textBoxQuantity_RAV.TextChanged += CalculateTotalCost_RAV;
        }

        private Panel panelMain_RAV;
        private TextBox textBoxOrderNumber_RAV;
        private TextBox textBoxExecutionDate_RAV;
        private TextBox textBoxOrderCost_RAV;
        private TextBox textBoxProductName_RAV;
        private TextBox textBoxProductPrice_RAV;
        private TextBox textBoxQuantity_RAV;
        private TextBox textBoxClientLastName_RAV;
        private TextBox textBoxClientFirstName_RAV;
        private TextBox textBoxClientMiddleName_RAV;
        private TextBox textBoxAccountNumber_RAV;
        private TextBox textBoxAddress_RAV;
        private TextBox textBoxPhone_RAV;

        private void InitializeFormFields()
        {
            int y = 20;
            int labelWidth = 200;
            int textBoxWidth = 350;

            // Разделитель: Информация о заказе
            AddSectionLabel("📋 ИНФОРМАЦИЯ О ЗАКАЗЕ", ref y);

            y += 5;

            AddLabeledTextBox("№ заказа:", ref y, labelWidth, textBoxWidth,
                out textBoxOrderNumber_RAV, true);
            AddLabeledTextBox("Дата исполнения (yyyy-MM-dd):", ref y, labelWidth, textBoxWidth,
                out textBoxExecutionDate_RAV);

            // Разделитель: Информация о товаре
            AddSectionLabel("📦 ИНФОРМАЦИЯ О ТОВАРЕ", ref y);

            y += 5;

            AddLabeledTextBox("Название товара:", ref y, labelWidth, textBoxWidth,
                out textBoxProductName_RAV);
            AddLabeledTextBox("Цена товара (₽):", ref y, labelWidth, textBoxWidth,
                out textBoxProductPrice_RAV);
            AddLabeledTextBox("Количество:", ref y, labelWidth, textBoxWidth,
                out textBoxQuantity_RAV);
            AddLabeledTextBox("Стоимость заказа (₽):", ref y, labelWidth, textBoxWidth,
                out textBoxOrderCost_RAV, true);

            // Разделитель: Информация о клиенте
            AddSectionLabel("👤 ИНФОРМАЦИЯ О КЛИЕНТЕ", ref y);

            y += 5;

            AddLabeledTextBox("Фамилия:", ref y, labelWidth, textBoxWidth,
                out textBoxClientLastName_RAV);
            AddLabeledTextBox("Имя:", ref y, labelWidth, textBoxWidth,
                out textBoxClientFirstName_RAV);
            AddLabeledTextBox("Отчество:", ref y, labelWidth, textBoxWidth,
                out textBoxClientMiddleName_RAV);
            AddLabeledTextBox("№ Счета:", ref y, labelWidth, textBoxWidth,
                out textBoxAccountNumber_RAV);
            AddLabeledTextBox("Адрес:", ref y, labelWidth, textBoxWidth,
                out textBoxAddress_RAV);
            AddLabeledTextBox("Телефон:", ref y, labelWidth, textBoxWidth,
                out textBoxPhone_RAV);

            // Обновляем высоту основной панели
            panelMain_RAV.Height = y + 50;
        }

        private void AddSectionLabel(string text, ref int y)
        {
            y += 10;

            Label label = new Label();
            label.Text = text;
            label.Location = new Point(30, y);
            label.Size = new Size(670, 32);
            label.Font = new Font("Arial", 12, FontStyle.Bold);
            label.ForeColor = Color.FromArgb(51, 122, 183);
            label.BackColor = Color.FromArgb(230, 230, 230);
            label.Padding = new Padding(20, 8, 0, 0);

            panelMain_RAV.Controls.Add(label);
            y += 35;
        }

        private void AddLabeledTextBox(string labelText, ref int y, int labelWidth, int textBoxWidth,
                                      out TextBox textBox, bool readOnly = false)
        {
            Label label = new Label();
            label.Text = labelText;
            label.Location = new Point(30, y);
            label.Size = new Size(labelWidth, 30);
            label.Font = new Font("Arial", 10, FontStyle.Bold);
            label.ForeColor = Color.FromArgb(73, 80, 87);

            textBox = new TextBox();
            textBox.Location = new Point(labelWidth + 40, y);
            textBox.Size = new Size(textBoxWidth, 32);
            textBox.Font = new Font("Arial", 10);
            textBox.BorderStyle = BorderStyle.FixedSingle;
            textBox.ReadOnly = readOnly;

            panelMain_RAV.Controls.Add(label);
            panelMain_RAV.Controls.Add(textBox);

            y += 40;
        }

        private void FormEditOrder_RAV_Resize(object sender, EventArgs e)
        {
            if (panelMain_RAV != null)
            {
                // Обновляем размер основной панели при изменении размера формы
                panelMain_RAV.Width = this.ClientSize.Width - 20;

                // Обновляем ширину всех текстовых полей
                foreach (Control control in panelMain_RAV.Controls)
                {
                    if (control is TextBox textBox)
                    {
                        // Находим соответствующую метку
                        foreach (Control labelControl in panelMain_RAV.Controls)
                        {
                            if (labelControl is Label label && label.Text.EndsWith(":"))
                            {
                                // Проверяем, соответствует ли метка полю ввода
                                if (Math.Abs(label.Top - textBox.Top) < 5)
                                {
                                    textBox.Width = panelMain_RAV.Width - label.Width - 50;
                                    textBox.Left = label.Width + 40;
                                    break;
                                }
                            }
                        }
                    }
                    else if (control is Label label && label.BackColor == Color.FromArgb(230, 230, 230))
                    {
                        // Обновляем ширину разделителей
                        label.Width = panelMain_RAV.Width - 40;
                    }
                }
            }
        }

        private void ButtonFullScreen_Click_RAV(object sender, EventArgs e)
        {
            if (!isMaximized)
            {
                // Переходим в полноэкранный режим
                originalSize = this.Size;
                originalLocation = this.Location;

                this.WindowState = FormWindowState.Normal;
                this.FormBorderStyle = FormBorderStyle.None;
                this.WindowState = FormWindowState.Maximized;

                ((Button)sender).Text = "🗗";
                isMaximized = true;
            }
            else
            {
                // Возвращаемся к обычному режиму
                this.FormBorderStyle = FormBorderStyle.Sizable;
                this.WindowState = FormWindowState.Normal;
                this.Size = originalSize;
                this.Location = originalLocation;

                ((Button)sender).Text = "⛶";
                isMaximized = false;
            }
        }

        private void LoadOrderData()
        {
            textBoxOrderNumber_RAV.Text = order.OrderNumber.ToString();
            textBoxExecutionDate_RAV.Text = order.ExecutionDate.ToString("yyyy-MM-dd");
            textBoxOrderCost_RAV.Text = order.OrderCost.ToString("F2");
            textBoxProductName_RAV.Text = order.ProductName ?? "";
            textBoxProductPrice_RAV.Text = order.ProductPrice.ToString("F2");
            textBoxQuantity_RAV.Text = order.Quantity.ToString();
            textBoxClientLastName_RAV.Text = order.ClientLastName ?? "";
            textBoxClientFirstName_RAV.Text = order.ClientFirstName ?? "";
            textBoxClientMiddleName_RAV.Text = order.ClientMiddleName ?? "";
            textBoxAccountNumber_RAV.Text = order.AccountNumber ?? "";
            textBoxAddress_RAV.Text = order.Address ?? "";
            textBoxPhone_RAV.Text = order.Phone ?? "";
        }

        private void CalculateTotalCost_RAV(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(textBoxProductPrice_RAV.Text) &&
                    !string.IsNullOrWhiteSpace(textBoxQuantity_RAV.Text))
                {
                    decimal price = decimal.Parse(textBoxProductPrice_RAV.Text);
                    int quantity = int.Parse(textBoxQuantity_RAV.Text);
                    decimal total = price * quantity;
                    textBoxOrderCost_RAV.Text = total.ToString("F2");
                }
                else
                {
                    textBoxOrderCost_RAV.Text = "0.00";
                }
            }
            catch
            {
                textBoxOrderCost_RAV.Text = "0.00";
            }
        }

        private void ButtonSave_Click_RAV(object sender, EventArgs e)
        {
            try
            {
                // Валидация обязательных полей
                if (string.IsNullOrWhiteSpace(textBoxProductName_RAV.Text))
                {
                    MessageBox.Show("Название товара обязательно для заполнения!",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBoxProductName_RAV.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(textBoxClientLastName_RAV.Text))
                {
                    MessageBox.Show("Фамилия клиента обязательна для заполнения!",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBoxClientLastName_RAV.Focus();
                    return;
                }

                // Парсинг значений с проверкой
                order.OrderNumber = int.Parse(textBoxOrderNumber_RAV.Text);
                order.ExecutionDate = DateTime.Parse(textBoxExecutionDate_RAV.Text);
                order.OrderCost = decimal.Parse(textBoxOrderCost_RAV.Text);
                order.ProductName = textBoxProductName_RAV.Text.Trim();
                order.ProductPrice = decimal.Parse(textBoxProductPrice_RAV.Text);
                order.Quantity = int.Parse(textBoxQuantity_RAV.Text);
                order.ClientLastName = textBoxClientLastName_RAV.Text.Trim();
                order.ClientFirstName = textBoxClientFirstName_RAV.Text.Trim();
                order.ClientMiddleName = textBoxClientMiddleName_RAV.Text.Trim();
                order.AccountNumber = textBoxAccountNumber_RAV.Text.Trim();
                order.Address = textBoxAddress_RAV.Text.Trim();
                order.Phone = textBoxPhone_RAV.Text.Trim();

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (FormatException)
            {
                MessageBox.Show("Проверьте правильность ввода числовых значений:\n" +
                              "- Цена товара должна быть числом (например: 1500.50)\n" +
                              "- Количество должно быть целым числом\n" +
                              "- Дата должна быть в формате yyyy-MM-dd",
                    "Ошибка формата", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public DataService.Order GetOrder()
        {
            return order;
        }

        // Переопределяем обработку клавиш для полноэкранного режима
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.F11 || keyData == (Keys.Alt | Keys.Enter))
            {
                ToggleFullScreen();
                return true;
            }
            else if (keyData == Keys.Escape && isMaximized)
            {
                ExitFullScreen();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void ToggleFullScreen()
        {
            ButtonFullScreen_Click_RAV(null, EventArgs.Empty);
        }

        private void ExitFullScreen()
        {
            if (isMaximized)
            {
                ButtonFullScreen_Click_RAV(null, EventArgs.Empty);
            }
        }

        // Защита от закрытия формы по ошибке
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (isMaximized)
                {
                    ExitFullScreen();
                    e.Cancel = true;
                }
            }
            base.OnFormClosing(e);
        }
    }
}