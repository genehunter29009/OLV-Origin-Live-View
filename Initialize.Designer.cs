namespace OriginLiveView
{
    partial class Initialize
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Initialize));
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxLatitude = new System.Windows.Forms.TextBox();
            this.textBoxLongitude = new System.Windows.Forms.TextBox();
            this.buttonInitialize = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.CheckBoxFakeInit = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.ComboBoxTimeZone = new System.Windows.Forms.ComboBox();
            this.CheckBoxSave = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label2.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label2.Location = new System.Drawing.Point(29, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "Latitude";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label3.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label3.Location = new System.Drawing.Point(29, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 20);
            this.label3.TabIndex = 2;
            this.label3.Text = "Longitude";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label5.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label5.Location = new System.Drawing.Point(29, 125);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 20);
            this.label5.TabIndex = 4;
            this.label5.Text = "TimeZone";
            // 
            // textBoxLatitude
            // 
            this.textBoxLatitude.Location = new System.Drawing.Point(152, 28);
            this.textBoxLatitude.Name = "textBoxLatitude";
            this.textBoxLatitude.Size = new System.Drawing.Size(196, 26);
            this.textBoxLatitude.TabIndex = 7;
            this.textBoxLatitude.Text = "-0.5310888889";
            this.textBoxLatitude.TextChanged += new System.EventHandler(this.textBoxLatitude_TextChanged);
            // 
            // textBoxLongitude
            // 
            this.textBoxLongitude.Location = new System.Drawing.Point(152, 74);
            this.textBoxLongitude.Name = "textBoxLongitude";
            this.textBoxLongitude.Size = new System.Drawing.Size(196, 26);
            this.textBoxLongitude.TabIndex = 8;
            this.textBoxLongitude.Text = "-80.4405583333";
            // 
            // buttonInitialize
            // 
            this.buttonInitialize.Location = new System.Drawing.Point(42, 204);
            this.buttonInitialize.Name = "buttonInitialize";
            this.buttonInitialize.Size = new System.Drawing.Size(133, 40);
            this.buttonInitialize.TabIndex = 12;
            this.buttonInitialize.Text = "Initialize";
            this.buttonInitialize.UseVisualStyleBackColor = true;
            this.buttonInitialize.Click += new System.EventHandler(this.button1_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(201, 204);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(147, 40);
            this.buttonCancel.TabIndex = 13;
            this.buttonCancel.Text = "Close";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.button2_Click);
            // 
            // CheckBoxFakeInit
            // 
            this.CheckBoxFakeInit.AutoSize = true;
            this.CheckBoxFakeInit.Location = new System.Drawing.Point(152, 163);
            this.CheckBoxFakeInit.Name = "CheckBoxFakeInit";
            this.CheckBoxFakeInit.Size = new System.Drawing.Size(113, 24);
            this.CheckBoxFakeInit.TabIndex = 14;
            this.CheckBoxFakeInit.Text = "checkBox1";
            this.CheckBoxFakeInit.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label4.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label4.Location = new System.Drawing.Point(38, 163);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(96, 20);
            this.label4.TabIndex = 16;
            this.label4.Text = "Demo Mode";
            // 
            // ComboBoxTimeZone
            // 
            this.ComboBoxTimeZone.FormattingEnabled = true;
            this.ComboBoxTimeZone.Items.AddRange(new object[] {
            "Africa/Abidjan",
            "Africa/Algiers",
            "Africa/Bissau",
            "Africa/Cairo",
            "Africa/Casablanca",
            "Africa/Ceuta",
            "Africa/El_Aaiun",
            "Africa/Johannesburg",
            "Africa/Juba",
            "Africa/Khartoum",
            "Africa/Lagos",
            "Africa/Maputo",
            "Africa/Monrovia",
            "Africa/Nairobi",
            "Africa/Ndjamena",
            "Africa/Sao_Tome",
            "Africa/Tripoli",
            "Africa/Tunis",
            "Africa/Windhoek",
            "America/Adak",
            "America/Anchorage",
            "America/Araguaina",
            "America/Argentina/Buenos_Aires",
            "America/Argentina/Catamarca",
            "America/Argentina/Cordoba",
            "America/Argentina/Jujuy",
            "America/Argentina/La_Rioja",
            "America/Argentina/Mendoza",
            "America/Argentina/Rio_Gallegos",
            "America/Argentina/Salta",
            "America/Argentina/San_Juan",
            "America/Argentina/San_Luis",
            "America/Argentina/Tucuman",
            "America/Argentina/Ushuaia",
            "America/Asuncion",
            "America/Bahia",
            "America/Bahia_Banderas",
            "America/Barbados",
            "America/Belem",
            "America/Belize",
            "America/Boa_Vista",
            "America/Bogota",
            "America/Boise",
            "America/Cambridge_Bay",
            "America/Campo_Grande",
            "America/Cancun",
            "America/Caracas",
            "America/Cayenne",
            "America/Chicago",
            "America/Chihuahua",
            "America/Ciudad_Juarez",
            "America/Costa_Rica",
            "America/Coyhaique",
            "America/Cuiaba",
            "America/Danmarkshavn",
            "America/Dawson",
            "America/Dawson_Creek",
            "America/Denver",
            "America/Detroit",
            "America/Edmonton",
            "America/Eirunepe",
            "America/El_Salvador",
            "America/Fort_Nelson",
            "America/Fortaleza",
            "America/Glace_Bay",
            "America/Goose_Bay",
            "America/Grand_Turk",
            "America/Guatemala",
            "America/Guayaquil",
            "America/Guyana",
            "America/Halifax",
            "America/Havana",
            "America/Hermosillo",
            "America/Indiana/Indianapolis",
            "America/Indiana/Knox",
            "America/Indiana/Marengo",
            "America/Indiana/Petersburg",
            "America/Indiana/Tell_City",
            "America/Indiana/Vevay",
            "America/Indiana/Vincennes",
            "America/Indiana/Winamac",
            "America/Inuvik",
            "America/Iqaluit",
            "America/Jamaica",
            "America/Juneau",
            "America/Kentucky/Louisville",
            "America/Kentucky/Monticello",
            "America/La_Paz",
            "America/Lima",
            "America/Los_Angeles",
            "America/Maceio",
            "America/Managua",
            "America/Manaus",
            "America/Martinique",
            "America/Matamoros",
            "America/Mazatlan",
            "America/Menominee",
            "America/Merida",
            "America/Metlakatla",
            "America/Mexico_City",
            "America/Miquelon",
            "America/Moncton",
            "America/Monterrey",
            "America/Montevideo",
            "America/New_York",
            "America/Nome",
            "America/Noronha",
            "America/North_Dakota/Beulah",
            "America/North_Dakota/Center",
            "America/North_Dakota/New_Salem",
            "America/Nuuk",
            "America/Ojinaga",
            "America/Panama",
            "America/Paramaribo",
            "America/Phoenix",
            "America/Port-au-Prince",
            "America/Porto_Velho",
            "America/Puerto_Rico",
            "America/Punta_Arenas",
            "America/Rankin_Inlet",
            "America/Recife",
            "America/Regina",
            "America/Resolute",
            "America/Rio_Branco",
            "America/Santarem",
            "America/Santiago",
            "America/Santo_Domingo",
            "America/Sao_Paulo",
            "America/Scoresbysund",
            "America/Sitka",
            "America/St_Johns",
            "America/Swift_Current",
            "America/Tegucigalpa",
            "America/Thule",
            "America/Tijuana",
            "America/Toronto",
            "America/Vancouver",
            "America/Whitehorse",
            "America/Winnipeg",
            "America/Yakutat",
            "Antarctica/Casey",
            "Antarctica/Davis",
            "Antarctica/Macquarie",
            "Antarctica/Mawson",
            "Antarctica/Palmer",
            "Antarctica/Rothera",
            "Antarctica/Troll",
            "Antarctica/Vostok",
            "Asia/Almaty",
            "Asia/Amman",
            "Asia/Anadyr",
            "Asia/Aqtau",
            "Asia/Aqtobe",
            "Asia/Ashgabat",
            "Asia/Atyrau",
            "Asia/Baghdad",
            "Asia/Baku",
            "Asia/Bangkok",
            "Asia/Barnaul",
            "Asia/Beirut",
            "Asia/Bishkek",
            "Asia/Chita",
            "Asia/Colombo",
            "Asia/Damascus",
            "Asia/Dhaka",
            "Asia/Dili",
            "Asia/Dubai",
            "Asia/Dushanbe",
            "Asia/Famagusta",
            "Asia/Gaza",
            "Asia/Hebron",
            "Asia/Ho_Chi_Minh",
            "Asia/Hong_Kong",
            "Asia/Hovd",
            "Asia/Irkutsk",
            "Asia/Jakarta",
            "Asia/Jayapura",
            "Asia/Jerusalem",
            "Asia/Kabul",
            "Asia/Kamchatka",
            "Asia/Karachi",
            "Asia/Kathmandu",
            "Asia/Khandyga",
            "Asia/Kolkata",
            "Asia/Krasnoyarsk",
            "Asia/Kuching",
            "Asia/Macau",
            "Asia/Magadan",
            "Asia/Makassar",
            "Asia/Manila",
            "Asia/Nicosia",
            "Asia/Novokuznetsk",
            "Asia/Novosibirsk",
            "Asia/Omsk",
            "Asia/Oral",
            "Asia/Pontianak",
            "Asia/Pyongyang",
            "Asia/Qatar",
            "Asia/Qostanay",
            "Asia/Qyzylorda",
            "Asia/Riyadh",
            "Asia/Sakhalin",
            "Asia/Samarkand",
            "Asia/Seoul",
            "Asia/Shanghai",
            "Asia/Singapore",
            "Asia/Srednekolymsk",
            "Asia/Taipei",
            "Asia/Tashkent",
            "Asia/Tbilisi",
            "Asia/Tehran",
            "Asia/Thimphu",
            "Asia/Tokyo",
            "Asia/Tomsk",
            "Asia/Ulaanbaatar",
            "Asia/Urumqi",
            "Asia/Ust-Nera",
            "Asia/Vladivostok",
            "Asia/Yakutsk",
            "Asia/Yangon",
            "Asia/Yekaterinburg",
            "Asia/Yerevan",
            "Atlantic/Azores",
            "Atlantic/Bermuda",
            "Atlantic/Canary",
            "Atlantic/Cape_Verde",
            "Atlantic/Faroe",
            "Atlantic/Madeira",
            "Atlantic/South_Georgia",
            "Atlantic/Stanley",
            "Australia/Adelaide",
            "Australia/Brisbane",
            "Australia/Broken_Hill",
            "Australia/Darwin",
            "Australia/Eucla",
            "Australia/Hobart",
            "Australia/Lindeman",
            "Australia/Lord_Howe",
            "Australia/Melbourne",
            "Australia/Perth",
            "Australia/Sydney",
            "Etc/GMT",
            "Etc/GMT-1",
            "Etc/GMT-10",
            "Etc/GMT-11",
            "Etc/GMT-12",
            "Etc/GMT-13",
            "Etc/GMT-14",
            "Etc/GMT-2",
            "Etc/GMT-3",
            "Etc/GMT-4",
            "Etc/GMT-5",
            "Etc/GMT-6",
            "Etc/GMT-7",
            "Etc/GMT-8",
            "Etc/GMT-9",
            "Etc/GMT+1",
            "Etc/GMT+10",
            "Etc/GMT+11",
            "Etc/GMT+12",
            "Etc/GMT+2",
            "Etc/GMT+3",
            "Etc/GMT+4",
            "Etc/GMT+5",
            "Etc/GMT+6",
            "Etc/GMT+7",
            "Etc/GMT+8",
            "Etc/GMT+9",
            "Etc/UTC",
            "Europe/Andorra",
            "Europe/Astrakhan",
            "Europe/Athens",
            "Europe/Belgrade",
            "Europe/Berlin",
            "Europe/Brussels",
            "Europe/Bucharest",
            "Europe/Budapest",
            "Europe/Chisinau",
            "Europe/Dublin",
            "Europe/Gibraltar",
            "Europe/Helsinki",
            "Europe/Istanbul",
            "Europe/Kaliningrad",
            "Europe/Kirov",
            "Europe/Kyiv",
            "Europe/Lisbon",
            "Europe/London",
            "Europe/Madrid",
            "Europe/Malta",
            "Europe/Minsk",
            "Europe/Moscow",
            "Europe/Paris",
            "Europe/Prague",
            "Europe/Riga",
            "Europe/Rome",
            "Europe/Samara",
            "Europe/Saratov",
            "Europe/Simferopol",
            "Europe/Sofia",
            "Europe/Tallinn",
            "Europe/Tirane",
            "Europe/Ulyanovsk",
            "Europe/Vienna",
            "Europe/Vilnius",
            "Europe/Volgograd",
            "Europe/Warsaw",
            "Europe/Zurich",
            "Indian/Chagos",
            "Indian/Maldives",
            "Indian/Mauritius",
            "Pacific/Apia",
            "Pacific/Auckland",
            "Pacific/Bougainville",
            "Pacific/Chatham",
            "Pacific/Easter",
            "Pacific/Efate",
            "Pacific/Fakaofo",
            "Pacific/Fiji",
            "Pacific/Galapagos",
            "Pacific/Gambier",
            "Pacific/Guadalcanal",
            "Pacific/Guam",
            "Pacific/Honolulu",
            "Pacific/Kanton",
            "Pacific/Kiritimati",
            "Pacific/Kosrae",
            "Pacific/Kwajalein",
            "Pacific/Marquesas",
            "Pacific/Nauru",
            "Pacific/Niue",
            "Pacific/Norfolk",
            "Pacific/Noumea",
            "Pacific/Pago_Pago",
            "Pacific/Palau",
            "Pacific/Pitcairn",
            "Pacific/Port_Moresby",
            "Pacific/Rarotonga",
            "Pacific/Tahiti",
            "Pacific/Tarawa",
            "Pacific/Tongatapu"});
            this.ComboBoxTimeZone.Location = new System.Drawing.Point(150, 118);
            this.ComboBoxTimeZone.Name = "ComboBoxTimeZone";
            this.ComboBoxTimeZone.Size = new System.Drawing.Size(198, 28);
            this.ComboBoxTimeZone.TabIndex = 17;
            this.ComboBoxTimeZone.Text = "America/Guayaquil";
            // 
            // CheckBoxSave
            // 
            this.CheckBoxSave.AutoSize = true;
            this.CheckBoxSave.Location = new System.Drawing.Point(192, 163);
            this.CheckBoxSave.Name = "CheckBoxSave";
            this.CheckBoxSave.Size = new System.Drawing.Size(113, 24);
            this.CheckBoxSave.TabIndex = 18;
            this.CheckBoxSave.Text = "checkBox1";
            this.CheckBoxSave.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label1.Location = new System.Drawing.Point(232, 164);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(108, 20);
            this.label1.TabIndex = 19;
            this.label1.Text = "Save Settings";
            // 
            // Initialize
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(376, 270);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CheckBoxSave);
            this.Controls.Add(this.ComboBoxTimeZone);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.CheckBoxFakeInit);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonInitialize);
            this.Controls.Add(this.textBoxLongitude);
            this.Controls.Add(this.textBoxLatitude);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(300, 700);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Initialize";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "  Initialize Origin";
            this.Load += new System.EventHandler(this.Initialize_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxLatitude;
        private System.Windows.Forms.TextBox textBoxLongitude;
        private System.Windows.Forms.Button buttonInitialize;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.CheckBox CheckBoxFakeInit;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox ComboBoxTimeZone;
        private System.Windows.Forms.CheckBox CheckBoxSave;
        private System.Windows.Forms.Label label1;
    }
}