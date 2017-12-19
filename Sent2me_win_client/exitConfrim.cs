using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Sent2me_win_client {
    class exitConfrim {
       public exitConfrim() {
            MessageBoxResult result = MessageBox.Show("คุณต้องการจะปิดโปรแกรมนี้ใช้ไหม?", "ออก", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes) {
                System.Windows.Application.Current.Shutdown();
            } else {
                return;
            }
        }
    }
}
