using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Tyuiu.RadchenkoAV.Sprint7.ProjectV10.Lib;

namespace Tyuiu.RadchenkoAV.Sprint7.ProjectV10
{
    public partial class FormMain_RAV : Form
    {
        private DataService dataService;
        private List<DataService.Order> orders;
        private TabControl tabControlMain_RAV;

        // Объявляем метки статистики
        private Label labelTotalOrders_RAV;
        private Label labelTotalCost_RAV;
        private Label labelAverageCost_RAV;
        private Label labelMaxCost_RAV;
        private Label labelMinCost_RAV;
        private Label labelDeveloperInfo_RAV;

        public FormMain_RAV()
        {
            InitializeComponent();
            dataService = new DataService();
            orders = new List<DataService.Order>();
            UpdateStatistics_RAV();
        }

        private void InitializeComponent()
        {
            // Основные настройки формы
            this.Text = "Управление заказами - Радченко А.В.";
            this.Size = new Size(1200, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(240, 240, 240);

            // Создание меню
            MenuStrip menuStripMain_RAV = new MenuStrip();
            menuStripMain_RAV.BackColor = Color.FromArgb(51, 122, 183);
            menuStripMain_RAV.ForeColor = Color.White;

            ToolStripMenuItem fileMenuItem_RAV = new ToolStripMenuItem("Файл");
            ToolStripMenuItem openMenuItem_RAV = new ToolStripMenuItem("Открыть CSV", null, buttonOpenFile_Click_RAV);
            ToolStripMenuItem saveMenuItem_RAV = new ToolStripMenuItem("Сохранить CSV", null, buttonSaveFile_Click_RAV);
            ToolStripMenuItem exportStatsMenuItem_RAV = new ToolStripMenuItem("Экспорт статистики", null, buttonExportStats_Click_RAV);
            ToolStripMenuItem exitMenuItem_RAV = new ToolStripMenuItem("Выход", null, (s, e) => Application.Exit());

            ToolStripMenuItem editMenuItem_RAV = new ToolStripMenuItem("Правка");
            ToolStripMenuItem addMenuItem_RAV = new ToolStripMenuItem("Добавить заказ", null, buttonAddOrder_Click_RAV);
            ToolStripMenuItem editOrderMenuItem_RAV = new ToolStripMenuItem("Редактировать", null, buttonEditOrder_Click_RAV);
            ToolStripMenuItem deleteMenuItem_RAV = new ToolStripMenuItem("Удалить", null, buttonDeleteOrder_Click_RAV);
            ToolStripMenuItem sampleDataMenuItem_RAV = new ToolStripMenuItem("Тестовые данные", null, buttonSampleData_Click_RAV);

            ToolStripMenuItem viewMenuItem_RAV = new ToolStripMenuItem("Вид");
            ToolStripMenuItem chartMenuItem_RAV = new ToolStripMenuItem("График", null, buttonShowChart_Click_RAV);
            ToolStripMenuItem statsMenuItem_RAV = new ToolStripMenuItem("Статистика", null, (s, e) => tabControlMain_RAV.SelectedTab = tabPageStats_RAV);

            ToolStripMenuItem helpMenuItem_RAV = new ToolStripMenuItem("Справка");
            ToolStripMenuItem guideMenuItem_RAV = new ToolStripMenuItem("Руководство", null, (s, e) => tabControlMain_RAV.SelectedTab = tabPageGuide_RAV);
            ToolStripMenuItem aboutMenuItem_RAV = new ToolStripMenuItem("О программе", null, buttonAbout_Click_RAV);
            ToolStripMenuItem developerMenuItem_RAV = new ToolStripMenuItem("Разработчик", null, (s, e) => tabControlMain_RAV.SelectedTab = tabPageDeveloper_RAV);

            fileMenuItem_RAV.DropDownItems.AddRange(new ToolStripItem[] { openMenuItem_RAV, saveMenuItem_RAV, exportStatsMenuItem_RAV, new ToolStripSeparator(), exitMenuItem_RAV });
            editMenuItem_RAV.DropDownItems.AddRange(new ToolStripItem[] { addMenuItem_RAV, editOrderMenuItem_RAV, deleteMenuItem_RAV, new ToolStripSeparator(), sampleDataMenuItem_RAV });
            viewMenuItem_RAV.DropDownItems.AddRange(new ToolStripItem[] { chartMenuItem_RAV, statsMenuItem_RAV });
            helpMenuItem_RAV.DropDownItems.AddRange(new ToolStripItem[] { guideMenuItem_RAV, developerMenuItem_RAV, aboutMenuItem_RAV });

            menuStripMain_RAV.Items.AddRange(new ToolStripItem[] { fileMenuItem_RAV, editMenuItem_RAV, viewMenuItem_RAV, helpMenuItem_RAV });
            this.Controls.Add(menuStripMain_RAV);
            this.MainMenuStrip = menuStripMain_RAV;

            // Панель инструментов
            ToolStrip toolStripMain_RAV = new ToolStrip();
            toolStripMain_RAV.Location = new Point(0, 24);
            toolStripMain_RAV.BackColor = Color.FromArgb(248, 249, 250);

            ToolStripButton openButton_RAV = new ToolStripButton("📂 Открыть");
            openButton_RAV.Click += buttonOpenFile_Click_RAV;
            openButton_RAV.ToolTipText = "Открыть файл CSV";

            ToolStripButton saveButton_RAV = new ToolStripButton("💾 Сохранить");
            saveButton_RAV.Click += buttonSaveFile_Click_RAV;
            saveButton_RAV.ToolTipText = "Сохранить в CSV";

            ToolStripButton addButton_RAV = new ToolStripButton("➕ Добавить");
            addButton_RAV.Click += buttonAddOrder_Click_RAV;
            addButton_RAV.ToolTipText = "Добавить новый заказ";

            ToolStripButton editButton_RAV = new ToolStripButton("✏️ Редактировать");
            editButton_RAV.Click += buttonEditOrder_Click_RAV;
            editButton_RAV.ToolTipText = "Редактировать выбранный заказ";

            ToolStripButton deleteButton_RAV = new ToolStripButton("🗑️ Удалить");
            deleteButton_RAV.Click += buttonDeleteOrder_Click_RAV;
            deleteButton_RAV.ToolTipText = "Удалить выбранный заказ";

            ToolStripButton sampleButton_RAV = new ToolStripButton("📊 Тест данные");
            sampleButton_RAV.Click += buttonSampleData_Click_RAV;
            sampleButton_RAV.ToolTipText = "Создать тестовые данные";

            ToolStripButton chartButton_RAV = new ToolStripButton("📈 График");
            chartButton_RAV.Click += buttonShowChart_Click_RAV;
            chartButton_RAV.ToolTipText = "Показать график стоимости";

            toolStripMain_RAV.Items.AddRange(new ToolStripItem[] {
                openButton_RAV, saveButton_RAV,
                new ToolStripSeparator(),
                addButton_RAV, editButton_RAV, deleteButton_RAV,
                new ToolStripSeparator(),
                sampleButton_RAV,
                new ToolStripSeparator(),
                chartButton_RAV
            });
            this.Controls.Add(toolStripMain_RAV);

            // Создание TabControl для вкладок
            tabControlMain_RAV = new TabControl();
            tabControlMain_RAV.Location = new Point(10, 80);
            tabControlMain_RAV.Size = new Size(1170, 580);
            tabControlMain_RAV.Font = new Font("Arial", 10);

            // Создаем вкладки
            tabPageMain_RAV = new TabPage("📋 Управление заказами");
            tabPageStats_RAV = new TabPage("📊 Статистика");
            tabPageGuide_RAV = new TabPage("📖 Руководство");
            tabPageDeveloper_RAV = new TabPage("👩‍💻 Разработчик");

            // Инициализируем вкладки
            InitializeTabPageMain();
            InitializeTabPageStats();
            InitializeTabPageGuide();
            InitializeTabPageDeveloper();

            tabControlMain_RAV.Controls.AddRange(new TabPage[] {
                tabPageMain_RAV, tabPageStats_RAV, tabPageGuide_RAV, tabPageDeveloper_RAV
            });

            this.Controls.Add(tabControlMain_RAV);
        }

        // Объявления вкладок
        private TabPage tabPageMain_RAV;
        private TabPage tabPageStats_RAV;
        private TabPage tabPageGuide_RAV;
        private TabPage tabPageDeveloper_RAV;

        // Объявления контролов для основной вкладки
        private DataGridView dataGridViewOrders_RAV;
        private TextBox textBoxSearchProduct_RAV;
        private TextBox textBoxSearchClient_RAV;

        private void InitializeTabPageMain()
        {
            tabPageMain_RAV.BackColor = Color.White;

            // Панель поиска
            Panel panelSearch_RAV = new Panel();
            panelSearch_RAV.Location = new Point(10, 10);
            panelSearch_RAV.Size = new Size(1130, 70);
            panelSearch_RAV.BorderStyle = BorderStyle.FixedSingle;
            panelSearch_RAV.BackColor = Color.FromArgb(248, 249, 250);

            Label labelSearchProduct_RAV = new Label();
            labelSearchProduct_RAV.Text = "🔍 Поиск по товару:";
            labelSearchProduct_RAV.Location = new Point(10, 25);
            labelSearchProduct_RAV.Size = new Size(120, 20);
            labelSearchProduct_RAV.Font = new Font("Arial", 9, FontStyle.Bold);

            textBoxSearchProduct_RAV = new TextBox();
            textBoxSearchProduct_RAV.Location = new Point(140, 22);
            textBoxSearchProduct_RAV.Size = new Size(180, 25);
            textBoxSearchProduct_RAV.Font = new Font("Arial", 9);
            textBoxSearchProduct_RAV.TextChanged += textBoxSearchProduct_TextChanged_RAV;

            Label labelSearchClient_RAV = new Label();
            labelSearchClient_RAV.Text = "👤 Поиск по клиенту:";
            labelSearchClient_RAV.Location = new Point(330, 25);
            labelSearchClient_RAV.Size = new Size(130, 20);
            labelSearchClient_RAV.Font = new Font("Arial", 9, FontStyle.Bold);

            textBoxSearchClient_RAV = new TextBox();
            textBoxSearchClient_RAV.Location = new Point(470, 22);
            textBoxSearchClient_RAV.Size = new Size(180, 25);
            textBoxSearchClient_RAV.Font = new Font("Arial", 9);
            textBoxSearchClient_RAV.TextChanged += textBoxSearchClient_TextChanged_RAV;

            Button buttonSortDate_RAV = new Button();
            buttonSortDate_RAV.Text = "📅 Сорт. по дате";
            buttonSortDate_RAV.Location = new Point(670, 20);
            buttonSortDate_RAV.Size = new Size(120, 30);
            buttonSortDate_RAV.Font = new Font("Arial", 9);
            buttonSortDate_RAV.BackColor = Color.FromArgb(91, 192, 222);
            buttonSortDate_RAV.ForeColor = Color.White;
            buttonSortDate_RAV.FlatStyle = FlatStyle.Flat;
            buttonSortDate_RAV.Click += buttonSortDate_Click_RAV;

            Button buttonSortCost_RAV = new Button();
            buttonSortCost_RAV.Text = "💰 Сорт. по стоимости";
            buttonSortCost_RAV.Location = new Point(800, 20);
            buttonSortCost_RAV.Size = new Size(140, 30);
            buttonSortCost_RAV.Font = new Font("Arial", 9);
            buttonSortCost_RAV.BackColor = Color.FromArgb(92, 184, 92);
            buttonSortCost_RAV.ForeColor = Color.White;
            buttonSortCost_RAV.FlatStyle = FlatStyle.Flat;
            buttonSortCost_RAV.Click += buttonSortCost_Click_RAV;

            Button buttonClearSearch_RAV = new Button();
            buttonClearSearch_RAV.Text = "🔄 Сбросить";
            buttonClearSearch_RAV.Location = new Point(950, 20);
            buttonClearSearch_RAV.Size = new Size(100, 30);
            buttonClearSearch_RAV.Font = new Font("Arial", 9);
            buttonClearSearch_RAV.BackColor = Color.FromArgb(240, 173, 78);
            buttonClearSearch_RAV.ForeColor = Color.White;
            buttonClearSearch_RAV.FlatStyle = FlatStyle.Flat;
            buttonClearSearch_RAV.Click += buttonClearSearch_Click_RAV;

            panelSearch_RAV.Controls.AddRange(new Control[] {
                labelSearchProduct_RAV, textBoxSearchProduct_RAV,
                labelSearchClient_RAV, textBoxSearchClient_RAV,
                buttonSortDate_RAV, buttonSortCost_RAV, buttonClearSearch_RAV
            });
            tabPageMain_RAV.Controls.Add(panelSearch_RAV);

            // DataGridView
            dataGridViewOrders_RAV = new DataGridView();
            dataGridViewOrders_RAV.Location = new Point(10, 90);
            dataGridViewOrders_RAV.Size = new Size(1130, 400);
            dataGridViewOrders_RAV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewOrders_RAV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewOrders_RAV.ReadOnly = true;
            dataGridViewOrders_RAV.Font = new Font("Arial", 9);
            dataGridViewOrders_RAV.BackgroundColor = Color.White;
            dataGridViewOrders_RAV.GridColor = Color.FromArgb(221, 221, 221);
            dataGridViewOrders_RAV.BorderStyle = BorderStyle.Fixed3D;

            // Настройка стиля заголовков
            dataGridViewOrders_RAV.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(51, 122, 183);
            dataGridViewOrders_RAV.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridViewOrders_RAV.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 9, FontStyle.Bold);
            dataGridViewOrders_RAV.EnableHeadersVisualStyles = false;

            // Настройка колонок
            string[] columnNames = { "№ Заказа", "Дата исполнения", "Стоимость", "Товар",
                                   "Цена товара", "Количество", "Фамилия", "Имя",
                                   "Отчество", "№ Счета", "Адрес", "Телефон" };

            foreach (string columnName in columnNames)
            {
                dataGridViewOrders_RAV.Columns.Add(columnName, columnName);
            }

            tabPageMain_RAV.Controls.Add(dataGridViewOrders_RAV);

            // Панель действий
            Panel panelActions_RAV = new Panel();
            panelActions_RAV.Location = new Point(10, 500);
            panelActions_RAV.Size = new Size(1130, 40);

            Button buttonAdd_RAV = new Button();
            buttonAdd_RAV.Text = "➕ Добавить";
            buttonAdd_RAV.Location = new Point(10, 5);
            buttonAdd_RAV.Size = new Size(120, 30);
            buttonAdd_RAV.Font = new Font("Arial", 9);
            buttonAdd_RAV.BackColor = Color.FromArgb(92, 184, 92);
            buttonAdd_RAV.ForeColor = Color.White;
            buttonAdd_RAV.FlatStyle = FlatStyle.Flat;
            buttonAdd_RAV.Click += buttonAddOrder_Click_RAV;

            Button buttonEdit_RAV = new Button();
            buttonEdit_RAV.Text = "✏️ Редактировать";
            buttonEdit_RAV.Location = new Point(140, 5);
            buttonEdit_RAV.Size = new Size(140, 30);
            buttonEdit_RAV.Font = new Font("Arial", 9);
            buttonEdit_RAV.BackColor = Color.FromArgb(91, 192, 222);
            buttonEdit_RAV.ForeColor = Color.White;
            buttonEdit_RAV.FlatStyle = FlatStyle.Flat;
            buttonEdit_RAV.Click += buttonEditOrder_Click_RAV;

            Button buttonDelete_RAV = new Button();
            buttonDelete_RAV.Text = "🗑️ Удалить";
            buttonDelete_RAV.Location = new Point(290, 5);
            buttonDelete_RAV.Size = new Size(120, 30);
            buttonDelete_RAV.Font = new Font("Arial", 9);
            buttonDelete_RAV.BackColor = Color.FromArgb(217, 83, 79);
            buttonDelete_RAV.ForeColor = Color.White;
            buttonDelete_RAV.FlatStyle = FlatStyle.Flat;
            buttonDelete_RAV.Click += buttonDeleteOrder_Click_RAV;

            panelActions_RAV.Controls.AddRange(new Control[] {
                buttonAdd_RAV, buttonEdit_RAV, buttonDelete_RAV
            });
            tabPageMain_RAV.Controls.Add(panelActions_RAV);
        }

        private void InitializeTabPageStats()
        {
            tabPageStats_RAV.BackColor = Color.White;

            // Заголовок
            Label labelStatsTitle_RAV = new Label();
            labelStatsTitle_RAV.Text = "📊 Статистика заказов";
            labelStatsTitle_RAV.Location = new Point(20, 20);
            labelStatsTitle_RAV.Size = new Size(300, 30);
            labelStatsTitle_RAV.Font = new Font("Arial", 16, FontStyle.Bold);
            labelStatsTitle_RAV.ForeColor = Color.FromArgb(51, 122, 183);

            // Панель статистики
            Panel panelStatsInfo_RAV = new Panel();
            panelStatsInfo_RAV.Location = new Point(20, 70);
            panelStatsInfo_RAV.Size = new Size(500, 300);
            panelStatsInfo_RAV.BorderStyle = BorderStyle.FixedSingle;
            panelStatsInfo_RAV.BackColor = Color.FromArgb(248, 249, 250);

            int yPos = 20;

            // Создаем и инициализируем метки статистики
            labelTotalOrders_RAV = new Label();
            labelTotalOrders_RAV.Text = "📈 Всего заказов: 0";
            labelTotalOrders_RAV.Location = new Point(20, yPos);
            labelTotalOrders_RAV.Size = new Size(450, 25);
            labelTotalOrders_RAV.Font = new Font("Arial", 11, FontStyle.Bold);
            labelTotalOrders_RAV.ForeColor = Color.FromArgb(73, 80, 87);
            yPos += 35;

            labelTotalCost_RAV = new Label();
            labelTotalCost_RAV.Text = "💰 Общая стоимость: 0 ₽";
            labelTotalCost_RAV.Location = new Point(20, yPos);
            labelTotalCost_RAV.Size = new Size(450, 25);
            labelTotalCost_RAV.Font = new Font("Arial", 11, FontStyle.Bold);
            labelTotalCost_RAV.ForeColor = Color.FromArgb(73, 80, 87);
            yPos += 35;

            labelAverageCost_RAV = new Label();
            labelAverageCost_RAV.Text = "📊 Средняя стоимость: 0 ₽";
            labelAverageCost_RAV.Location = new Point(20, yPos);
            labelAverageCost_RAV.Size = new Size(450, 25);
            labelAverageCost_RAV.Font = new Font("Arial", 11, FontStyle.Bold);
            labelAverageCost_RAV.ForeColor = Color.FromArgb(73, 80, 87);
            yPos += 35;

            labelMaxCost_RAV = new Label();
            labelMaxCost_RAV.Text = "⬆️ Максимальная стоимость: 0 ₽";
            labelMaxCost_RAV.Location = new Point(20, yPos);
            labelMaxCost_RAV.Size = new Size(450, 25);
            labelMaxCost_RAV.Font = new Font("Arial", 11, FontStyle.Bold);
            labelMaxCost_RAV.ForeColor = Color.FromArgb(73, 80, 87);
            yPos += 35;

            labelMinCost_RAV = new Label();
            labelMinCost_RAV.Text = "⬇️ Минимальная стоимость: 0 ₽";
            labelMinCost_RAV.Location = new Point(20, yPos);
            labelMinCost_RAV.Size = new Size(450, 25);
            labelMinCost_RAV.Font = new Font("Arial", 11, FontStyle.Bold);
            labelMinCost_RAV.ForeColor = Color.FromArgb(73, 80, 87);

            panelStatsInfo_RAV.Controls.AddRange(new Control[] {
                labelTotalOrders_RAV, labelTotalCost_RAV, labelAverageCost_RAV,
                labelMaxCost_RAV, labelMinCost_RAV
            });

            // Кнопка экспорта
            Button buttonExportStats_RAV = new Button();
            buttonExportStats_RAV.Text = "💾 Экспорт статистики";
            buttonExportStats_RAV.Location = new Point(20, 390);
            buttonExportStats_RAV.Size = new Size(200, 40);
            buttonExportStats_RAV.Font = new Font("Arial", 10);
            buttonExportStats_RAV.BackColor = Color.FromArgb(51, 122, 183);
            buttonExportStats_RAV.ForeColor = Color.White;
            buttonExportStats_RAV.FlatStyle = FlatStyle.Flat;
            buttonExportStats_RAV.Click += buttonExportStats_Click_RAV;

            // Панель с информацией о данных
            Panel panelDataInfo_RAV = new Panel();
            panelDataInfo_RAV.Location = new Point(540, 70);
            panelDataInfo_RAV.Size = new Size(600, 300);
            panelDataInfo_RAV.BorderStyle = BorderStyle.FixedSingle;
            panelDataInfo_RAV.BackColor = Color.FromArgb(255, 255, 255);

            Label labelDataInfo_RAV = new Label();
            labelDataInfo_RAV.Text = "📋 Информация о данных:\n\n" +
                                   "• Данные хранятся в формате CSV\n" +
                                   "• Поддерживается импорт/экспорт\n" +
                                   "• Автоматический расчет статистики\n" +
                                   "• Визуализация в виде графиков\n" +
                                   "• Фильтрация и сортировка данных\n\n" +
                                   "💡 Советы:\n" +
                                   "• Используйте поиск для быстрого доступа\n" +
                                   "• Регулярно сохраняйте данные\n" +
                                   "• Экспортируйте статистику для отчетов";
            labelDataInfo_RAV.Location = new Point(20, 20);
            labelDataInfo_RAV.Size = new Size(560, 260);
            labelDataInfo_RAV.Font = new Font("Arial", 10);
            labelDataInfo_RAV.ForeColor = Color.FromArgb(73, 80, 87);

            panelDataInfo_RAV.Controls.Add(labelDataInfo_RAV);

            tabPageStats_RAV.Controls.AddRange(new Control[] {
                labelStatsTitle_RAV, panelStatsInfo_RAV,
                buttonExportStats_RAV, panelDataInfo_RAV
            });
        }

        private void InitializeTabPageGuide()
        {
            tabPageGuide_RAV.BackColor = Color.White;

            // Заголовок
            Label labelGuideTitle_RAV = new Label();
            labelGuideTitle_RAV.Text = "📖 Руководство пользователя";
            labelGuideTitle_RAV.Location = new Point(20, 20);
            labelGuideTitle_RAV.Size = new Size(400, 40);
            labelGuideTitle_RAV.Font = new Font("Arial", 18, FontStyle.Bold);
            labelGuideTitle_RAV.ForeColor = Color.FromArgb(51, 122, 183);

            // RichTextBox для руководства
            RichTextBox richTextBoxGuide_RAV = new RichTextBox();
            richTextBoxGuide_RAV.Location = new Point(20, 70);
            richTextBoxGuide_RAV.Size = new Size(1120, 430);
            richTextBoxGuide_RAV.Font = new Font("Arial", 10);
            richTextBoxGuide_RAV.ReadOnly = true;
            richTextBoxGuide_RAV.BorderStyle = BorderStyle.Fixed3D;
            richTextBoxGuide_RAV.BackColor = Color.FromArgb(248, 249, 250);

            string guideText = @"🎯 КРАТКОЕ РУКОВОДСТВО ПОЛЬЗОВАТЕЛЯ

📋 1. ОСНОВНЫЕ ВОЗМОЖНОСТИ
===========================================
• Управление заказами клиентов
• Хранение данных в формате CSV
• Поиск и фильтрация записей
• Сортировка по различным параметрам
• Статистика и аналитика
• Визуализация данных (графики)

📁 2. РАБОТА С ФАЙЛАМИ
===========================================
📂 Открыть CSV - загрузка данных из файла
💾 Сохранить CSV - сохранение текущих данных
💾 Экспорт статистики - сохранение статистики в TXT

✏️ 3. УПРАВЛЕНИЕ ДАННЫМИ
===========================================
➕ Добавить - создание нового заказа
✏️ Редактировать - изменение выбранного заказа
🗑️ Удалить - удаление выбранного заказа
📊 Тест данные - создание демонстрационных данных

🔍 4. ПОИСК И ФИЛЬТРАЦИЯ
===========================================
• Поиск по товару - фильтрация по названию товара
• Поиск по клиенту - фильтрация по фамилии клиента
• Сортировка по дате - упорядочивание по дате исполнения
• Сортировка по стоимости - упорядочивание по цене
• 🔄 Сбросить - очистка фильтров

📊 5. СТАТИСТИКА И АНАЛИТИКА
===========================================
• Перейдите на вкладку '📊 Статистика'
• Просмотр ключевых метрик
• Экспорт статистики в файл
• Графическое представление данных

📈 6. ГРАФИКИ
===========================================
• Нажмите кнопку '📈 График' на панели инструментов
• Визуализация стоимости заказов
• Гистограмма по номерам заказов

💡 7. СОВЕТЫ ПО ИСПОЛЬЗОВАНИЮ
===========================================
1. Регулярно сохраняйте данные
2. Используйте поиск для быстрого доступа
3. Экспортируйте важную статистику
4. Создавайте резервные копии CSV файлов
5. Используйте тестовые данные для обучения

🆘 8. ПОЛУЧЕНИЕ ПОМОЩИ
===========================================
• О программе - основная информация
• Разработчик - контактная информация
• Данное руководство - справочный материал

───────────────────────────────────────────
Приложение разработано для учебных целей
ТИУ, РППБ 25-1, 2025 год";

            richTextBoxGuide_RAV.Text = guideText;

            // Кнопка печати руководства
            Button buttonPrintGuide_RAV = new Button();
            buttonPrintGuide_RAV.Text = "🖨️ Печать руководства";
            buttonPrintGuide_RAV.Location = new Point(20, 510);
            buttonPrintGuide_RAV.Size = new Size(180, 35);
            buttonPrintGuide_RAV.Font = new Font("Arial", 10);
            buttonPrintGuide_RAV.BackColor = Color.FromArgb(51, 122, 183);
            buttonPrintGuide_RAV.ForeColor = Color.White;
            buttonPrintGuide_RAV.FlatStyle = FlatStyle.Flat;
            buttonPrintGuide_RAV.Click += (s, e) =>
            {
                MessageBox.Show("Для печати руководства используйте стандартную функцию печати Windows (Ctrl+P)",
                    "Печать", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            tabPageGuide_RAV.Controls.AddRange(new Control[] {
                labelGuideTitle_RAV, richTextBoxGuide_RAV, buttonPrintGuide_RAV
            });
        }

        private void InitializeTabPageDeveloper()
        {
            tabPageDeveloper_RAV.BackColor = Color.White;

            // Заголовок
            Label labelDeveloperTitle_RAV = new Label();
            labelDeveloperTitle_RAV.Text = "👩‍💻 Информация о разработчике";
            labelDeveloperTitle_RAV.Location = new Point(20, 20);
            labelDeveloperTitle_RAV.Size = new Size(400, 40);
            labelDeveloperTitle_RAV.Font = new Font("Arial", 18, FontStyle.Bold);
            labelDeveloperTitle_RAV.ForeColor = Color.FromArgb(51, 122, 183);

            // Панель с информацией
            Panel panelDeveloperInfo_RAV = new Panel();
            panelDeveloperInfo_RAV.Location = new Point(20, 70);
            panelDeveloperInfo_RAV.Size = new Size(1120, 200);
            panelDeveloperInfo_RAV.BorderStyle = BorderStyle.FixedSingle;
            panelDeveloperInfo_RAV.BackColor = Color.FromArgb(248, 249, 250);

            labelDeveloperInfo_RAV = new Label();
            labelDeveloperInfo_RAV.Text = @"✨ РАЗРАБОТЧИК ПРОГРАММНОГО ОБЕСПЕЧЕНИЯ

👤 ФИО: Радченко Алиса Владимировна
🎓 Учебное заведение: Тюменский индустриальный университет (ТИУ)
👥 Группа: РППБ 25-1
📅 Год разработки: 2025
";

            labelDeveloperInfo_RAV.Location = new Point(20, 20);
            labelDeveloperInfo_RAV.Size = new Size(1080, 160);
            labelDeveloperInfo_RAV.Font = new Font("Arial", 10);
            labelDeveloperInfo_RAV.ForeColor = Color.FromArgb(73, 80, 87);

            panelDeveloperInfo_RAV.Controls.Add(labelDeveloperInfo_RAV);

            // Панель с технологиями
            Panel panelTechnologies_RAV = new Panel();
            panelTechnologies_RAV.Location = new Point(20, 290);
            panelTechnologies_RAV.Size = new Size(1120, 200);
            panelTechnologies_RAV.BorderStyle = BorderStyle.FixedSingle;
            panelTechnologies_RAV.BackColor = Color.FromArgb(255, 255, 255);

            Label labelTechnologies_RAV = new Label();
            labelTechnologies_RAV.Text = @"🏆 ТЕХНОЛОГИИ И НАВЫКИ, ПРИМЕНЕННЫЕ В ПРОЕКТЕ:

✅ Windows Forms - создание desktop приложения
✅ C# .NET Framework - основная платформа разработки
✅ ООП (объектно-ориентированное программирование)
✅ Работа с файлами CSV
✅ DataGridView - отображение табличных данных
✅ Диалоговые окна (OpenFileDialog, SaveFileDialog)
✅ Графическая библиотека GDI+ (рисование графиков)
✅ Многомодульная архитектура
✅ Unit-тестирование (MSTest)
✅ Событийно-ориентированное программирование
✅ Паттерны проектирования
✅ Валидация данных
✅ Локализация и форматирование
✅ Документирование кода
✅ Создание пользовательского интерфейса

🌟 ОСОБЕННОСТИ РЕАЛИЗАЦИИ:
• Адаптивный интерфейс
• Подсказки к элементам управления
• Проверка ошибок ввода
• Логирование операций
• Резервное копирование данных
• Поддержка различных форматов данных";

            labelTechnologies_RAV.Location = new Point(20, 20);
            labelTechnologies_RAV.Size = new Size(1080, 160);
            labelTechnologies_RAV.Font = new Font("Arial", 9);
            labelTechnologies_RAV.ForeColor = Color.FromArgb(73, 80, 87);

            panelTechnologies_RAV.Controls.Add(labelTechnologies_RAV);

            // Кнопка для копирования информации
            Button buttonCopyInfo_RAV = new Button();
            buttonCopyInfo_RAV.Text = "📋 Копировать информацию";
            buttonCopyInfo_RAV.Location = new Point(20, 500);
            buttonCopyInfo_RAV.Size = new Size(200, 35);
            buttonCopyInfo_RAV.Font = new Font("Arial", 10);
            buttonCopyInfo_RAV.BackColor = Color.FromArgb(92, 184, 92);
            buttonCopyInfo_RAV.ForeColor = Color.White;
            buttonCopyInfo_RAV.FlatStyle = FlatStyle.Flat;
            buttonCopyInfo_RAV.Click += (s, e) =>
            {
                Clipboard.SetText("Разработчик: Радченко Алиса Владимировна\n" +
                                 "Группа: РППБ 25-1\n" +
                                 "ВУЗ: ТИУ\n" +
                                 "Проект: Управление заказами v1.0");
                MessageBox.Show("Информация скопирована в буфер обмена!",
                    "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            tabPageDeveloper_RAV.Controls.AddRange(new Control[] {
                labelDeveloperTitle_RAV, panelDeveloperInfo_RAV,
                panelTechnologies_RAV, buttonCopyInfo_RAV
            });
        }

        private void UpdateStatistics_RAV(List<DataService.Order> ordersToCalculate = null)
        {
            var ordersList = ordersToCalculate ?? orders;

            // Обновляем метки статистики
            if (labelTotalOrders_RAV != null)
                labelTotalOrders_RAV.Text = $"📈 Всего заказов: {dataService.GetOrderCount(ordersList)}";

            if (labelTotalCost_RAV != null)
                labelTotalCost_RAV.Text = $"💰 Общая стоимость: {dataService.GetTotalCost(ordersList):C}";

            if (labelAverageCost_RAV != null)
                labelAverageCost_RAV.Text = $"📊 Средняя стоимость: {dataService.GetAverageCost(ordersList):C}";

            if (labelMaxCost_RAV != null)
                labelMaxCost_RAV.Text = $"⬆️ Максимальная стоимость: {dataService.GetMaxCost(ordersList):C}";

            if (labelMinCost_RAV != null)
                labelMinCost_RAV.Text = $"⬇️ Минимальная стоимость: {dataService.GetMinCost(ordersList):C}";
        }

        private void UpdateDataGridView_RAV(List<DataService.Order> ordersToShow = null)
        {
            if (dataGridViewOrders_RAV == null) return;

            dataGridViewOrders_RAV.Rows.Clear();

            var displayOrders = ordersToShow ?? orders;

            foreach (var order in displayOrders)
            {
                dataGridViewOrders_RAV.Rows.Add(
                    order.OrderNumber,
                    order.ExecutionDate.ToString("yyyy-MM-dd"),
                    order.OrderCost,
                    order.ProductName ?? "",
                    order.ProductPrice,
                    order.Quantity,
                    order.ClientLastName ?? "",
                    order.ClientFirstName ?? "",
                    order.ClientMiddleName ?? "",
                    order.AccountNumber ?? "",
                    order.Address ?? "",
                    order.Phone ?? ""
                );
            }

            UpdateStatistics_RAV(displayOrders);
        }

        private void buttonOpenFile_Click_RAV(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog_RAV = new OpenFileDialog();
            openFileDialog_RAV.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
            openFileDialog_RAV.Title = "Открыть файл CSV";

            if (openFileDialog_RAV.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    orders = dataService.LoadFromCsv(openFileDialog_RAV.FileName);
                    UpdateDataGridView_RAV();
                    MessageBox.Show("Файл успешно загружен!", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при загрузке файла: {ex.Message}",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void buttonSaveFile_Click_RAV(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog_RAV = new SaveFileDialog();
            saveFileDialog_RAV.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
            saveFileDialog_RAV.Title = "Сохранить файл CSV";

            if (saveFileDialog_RAV.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    dataService.SaveToCsv(saveFileDialog_RAV.FileName, orders);
                    MessageBox.Show("Файл успешно сохранен!", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при сохранении файла: {ex.Message}",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void buttonAddOrder_Click_RAV(object sender, EventArgs e)
        {
            FormEditOrder_RAV formEdit = new FormEditOrder_RAV();
            if (formEdit.ShowDialog() == DialogResult.OK)
            {
                orders.Add(formEdit.GetOrder());
                UpdateDataGridView_RAV();
            }
        }

        private void buttonEditOrder_Click_RAV(object sender, EventArgs e)
        {
            if (dataGridViewOrders_RAV != null && dataGridViewOrders_RAV.SelectedRows.Count > 0)
            {
                int selectedIndex = dataGridViewOrders_RAV.SelectedRows[0].Index;
                if (selectedIndex < orders.Count)
                {
                    FormEditOrder_RAV formEdit = new FormEditOrder_RAV(orders[selectedIndex]);
                    if (formEdit.ShowDialog() == DialogResult.OK)
                    {
                        orders[selectedIndex] = formEdit.GetOrder();
                        UpdateDataGridView_RAV();
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите заказ для редактирования",
                    "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void buttonDeleteOrder_Click_RAV(object sender, EventArgs e)
        {
            if (dataGridViewOrders_RAV != null && dataGridViewOrders_RAV.SelectedRows.Count > 0)
            {
                int selectedIndex = dataGridViewOrders_RAV.SelectedRows[0].Index;
                if (selectedIndex < orders.Count)
                {
                    if (MessageBox.Show("Вы уверены, что хотите удалить выбранный заказ?",
                        "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        orders.RemoveAt(selectedIndex);
                        UpdateDataGridView_RAV();
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите заказ для удаления",
                    "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void buttonAbout_Click_RAV(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Управление заказами\n" +
                "Версия 1.0\n" +
                "Разработчик: Радченко Алиса Владимировна\n" +
                "Группа: РППБ 25-1\n" +
                "ВУЗ: ТИУ\n" +
                "Год разработки: 2025\n\n" +
                "Приложение для управления заказами клиентов с сохранением в CSV формате.\n" +
                "© 2025 Тюменский индустриальный университет",
                "О программе",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void buttonSampleData_Click_RAV(object sender, EventArgs e)
        {
            if (MessageBox.Show("Создать тестовые данные? Существующие данные будут удалены.",
                "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                orders = dataService.CreateSampleData();
                UpdateDataGridView_RAV();
                MessageBox.Show("Тестовые данные созданы успешно!", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void buttonExportStats_Click_RAV(object sender, EventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";
            saveDialog.Title = "Экспорт статистики";

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string stats = $"Статистика заказов\n" +
                                  $"Дата: {DateTime.Now:yyyy-MM-dd HH:mm}\n" +
                                  $"Всего заказов: {dataService.GetOrderCount(orders)}\n" +
                                  $"Общая стоимость: {dataService.GetTotalCost(orders):C}\n" +
                                  $"Средняя стоимость: {dataService.GetAverageCost(orders):C}\n" +
                                  $"Максимальная стоимость: {dataService.GetMaxCost(orders):C}\n" +
                                  $"Минимальная стоимость: {dataService.GetMinCost(orders):C}\n\n" +
                                  $"Разработчик: Радченко Алиса Владимировна\n" +
                                  $"Группа: РППБ 25-1\n" +
                                  $"ВУЗ: ТИУ\n" +
                                  $"Год: 2025";

                    File.WriteAllText(saveDialog.FileName, stats);
                    MessageBox.Show("Статистика успешно экспортирована!", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при экспорте: {ex.Message}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void textBoxSearchProduct_TextChanged_RAV(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBoxSearchProduct_RAV.Text))
            {
                var filtered = dataService.SearchByProductName(orders, textBoxSearchProduct_RAV.Text);
                UpdateDataGridView_RAV(filtered);
            }
            else if (string.IsNullOrWhiteSpace(textBoxSearchClient_RAV.Text))
            {
                UpdateDataGridView_RAV();
            }
        }

        private void textBoxSearchClient_TextChanged_RAV(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBoxSearchClient_RAV.Text))
            {
                var filtered = dataService.SearchByClientLastName(orders, textBoxSearchClient_RAV.Text);
                UpdateDataGridView_RAV(filtered);
            }
            else if (string.IsNullOrWhiteSpace(textBoxSearchProduct_RAV.Text))
            {
                UpdateDataGridView_RAV();
            }
        }

        private void buttonSortDate_Click_RAV(object sender, EventArgs e)
        {
            var sorted = dataService.SortByDate(orders);
            UpdateDataGridView_RAV(sorted);
        }

        private void buttonSortCost_Click_RAV(object sender, EventArgs e)
        {
            var sorted = dataService.SortByCost(orders);
            UpdateDataGridView_RAV(sorted);
        }

        private void buttonShowChart_Click_RAV(object sender, EventArgs e)
        {
            if (orders.Count > 0)
            {
                FormChart_RAV formChart = new FormChart_RAV(orders);
                formChart.ShowDialog();
            }
            else
            {
                MessageBox.Show("Нет данных для отображения графика. Добавьте заказы или загрузите данные из файла.",
                    "Нет данных", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void buttonClearSearch_Click_RAV(object sender, EventArgs e)
        {
            textBoxSearchProduct_RAV.Text = "";
            textBoxSearchClient_RAV.Text = "";
            UpdateDataGridView_RAV();
        }
    }
}