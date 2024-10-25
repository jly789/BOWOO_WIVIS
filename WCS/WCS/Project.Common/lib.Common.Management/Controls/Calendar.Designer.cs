namespace lib.Common.Management
{
    partial class Calendar
    {
        /// <summary> 
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.radDatePickerCal = new Telerik.WinControls.UI.RadDateTimePicker();
            ((System.ComponentModel.ISupportInitialize)(this.radDatePickerCal)).BeginInit();
            this.SuspendLayout();
            // 
            // radDatePickerCal
            // 
            this.radDatePickerCal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radDatePickerCal.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.radDatePickerCal.Location = new System.Drawing.Point(0, 0);
            this.radDatePickerCal.Name = "radDatePickerCal";
            this.radDatePickerCal.Size = new System.Drawing.Size(159, 20);
            this.radDatePickerCal.TabIndex = 0;
            this.radDatePickerCal.TabStop = false;
            this.radDatePickerCal.Text = "2022-04-20 오전 12:00";
            this.radDatePickerCal.Value = new System.DateTime(2022, 4, 20, 0, 0, 0, 0);
            // 
            // Calendar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.radDatePickerCal);
            this.Name = "Calendar";
            this.Size = new System.Drawing.Size(159, 24);
            ((System.ComponentModel.ISupportInitialize)(this.radDatePickerCal)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadDateTimePicker radDatePickerCal;
    }
}
