using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Peak_deconvolution_for_OES_and_Actinometry.Utills
{
    public static class LoadingManager
    {
        private static Loading _loadingForm;
        public static async Task<T> RunWithLoadingAsync<T>(Func<Task<T>> action, string message = "Working process")
        {
            // 로딩 폼 생성 (새 매개변수 없이)
            _loadingForm = new Loading(message);

            // 로딩 폼을 비동기적으로 표시
            _loadingForm.ShowLoading();

            // 로딩 폼이 표시될 시간을 주기 위해 약간 대기
            await Task.Delay(100);

            try
            {
                // 주 작업 실행
                return await action();
            }
            finally
            {
                // 작업 완료 후 로딩 폼 닫기
                _loadingForm.HideLoading();
            }
        }
        public static async Task RunWithLoadingAsync(Func<Task> action, string message = "Working process")
        {
            _loadingForm = new Loading(message);
            _loadingForm.ShowLoading();

            await Task.Delay(100);

            try
            {
                await action();
            }
            finally
            {
                _loadingForm.HideLoading();
            }
        }
        public static void UpdateMessage(string message)
        {
            if (_loadingForm != null && !_loadingForm.IsDisposed)
            {
                _loadingForm.SetMessage(message);
            }
        }
        public static void UpdateProgress(int value)
        {
            if (_loadingForm != null && !_loadingForm.IsDisposed)
            {
                _loadingForm.UpdateProgress(value);
            }
        }
        public static void ResetProgress()
        {
            if (_loadingForm != null && !_loadingForm.IsDisposed)
            {
                _loadingForm.UpdateProgress(0);
            }
        }
    }
}
