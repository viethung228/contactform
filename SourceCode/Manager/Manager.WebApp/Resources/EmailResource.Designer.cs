﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Manager.WebApp.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class EmailResource {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal EmailResource() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Manager.WebApp.Resources.EmailResource", typeof(EmailResource).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Không thể đồng bộ email [{0}] từ mail server [{1}]. Vui lòng kiểm tra lại cấu hình.
        /// </summary>
        public static string ERROR_EMAIL_COULD_NOT_SYNC_FORMAT {
            get {
                return ResourceManager.GetString("ERROR_EMAIL_COULD_NOT_SYNC_FORMAT", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Đăng nhập email [{0}] thất bại. Vui lòng kiểm tra thông tin đăng nhập.
        /// </summary>
        public static string ERROR_EMAIL_INVALID_LOGIN_FORMAT {
            get {
                return ResourceManager.GetString("ERROR_EMAIL_INVALID_LOGIN_FORMAT", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Quá trình đăng nhập bị gián đoạn. Vui lòng thử lại sau.
        /// </summary>
        public static string ERROR_EMAIL_LOGIN_DELAY_FORMAT {
            get {
                return ResourceManager.GetString("ERROR_EMAIL_LOGIN_DELAY_FORMAT", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Truy cập POP chưa được kích hoạt [{0}]. Vui lòng kiểm tra lại.
        /// </summary>
        public static string ERROR_EMAIL_POP_SERVER_LOCKED_FORMAT {
            get {
                return ResourceManager.GetString("ERROR_EMAIL_POP_SERVER_LOCKED_FORMAT", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Không tìm thấy máy chủ [{0}]. Vui lòng kiểm tra lại cấu hình.
        /// </summary>
        public static string ERROR_EMAIL_POP_SERVER_NOT_FOUND_FORMAT {
            get {
                return ResourceManager.GetString("ERROR_EMAIL_POP_SERVER_NOT_FOUND_FORMAT", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Email server [{0}] đang bận, vui lòng thử lại sau.
        /// </summary>
        public static string ERROR_FORMAT_SMTP_BUSY {
            get {
                return ResourceManager.GetString("ERROR_FORMAT_SMTP_BUSY", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Email [{0}] chưa được kích hoạt cơ chế gửi mail bằng SMPT, vui lòng kiểm tra lại.
        /// </summary>
        public static string ERROR_FORMAT_SMTP_CLIENT_NOT_PERMITTED {
            get {
                return ResourceManager.GetString("ERROR_FORMAT_SMTP_CLIENT_NOT_PERMITTED", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Nội dung quá dài để lưu trữ tại email server [{0}], vui lòng kiểm tra lại.
        /// </summary>
        public static string ERROR_FORMAT_SMTP_EMAIL_TOO_LARGE {
            get {
                return ResourceManager.GetString("ERROR_FORMAT_SMTP_EMAIL_TOO_LARGE", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Không tìm thấy email server [{0}].
        /// </summary>
        public static string ERROR_FORMAT_SMTP_NOT_FOUND {
            get {
                return ResourceManager.GetString("ERROR_FORMAT_SMTP_NOT_FOUND", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Email server [{0}] cần sử dụng phương thức bảo mật SSL.
        /// </summary>
        public static string ERROR_FORMAT_SMTP_SSL_REQUIRED {
            get {
                return ResourceManager.GetString("ERROR_FORMAT_SMTP_SSL_REQUIRED", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Lỗi khi thực hiện gửi email.
        /// </summary>
        public static string ERROR_SENDING_EMAIL_FAILED {
            get {
                return ResourceManager.GetString("ERROR_SENDING_EMAIL_FAILED", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Thông tin tài khoản.
        /// </summary>
        public static string LB_ACCOUNT_INFO {
            get {
                return ResourceManager.GetString("LB_ACCOUNT_INFO", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Vui lòng click vào Link xác nhận ở trên để hoàn tất đăng ký tài khoản.
        /// </summary>
        public static string LB_ACTIVE_ACCOUNT_DES {
            get {
                return ResourceManager.GetString("LB_ACTIVE_ACCOUNT_DES", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Xin chào bạn, cảm ơn bạn đã đăng ký sử dụng hệ thống NihonWork.
        /// </summary>
        public static string LB_BODY_HELLO {
            get {
                return ResourceManager.GetString("LB_BODY_HELLO", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Link xác nhận.
        /// </summary>
        public static string LB_CONFIRM_LINK {
            get {
                return ResourceManager.GetString("LB_CONFIRM_LINK", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Email.
        /// </summary>
        public static string LB_EMAIL {
            get {
                return ResourceManager.GetString("LB_EMAIL", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Tại đây.
        /// </summary>
        public static string LB_IN_HERE {
            get {
                return ResourceManager.GetString("LB_IN_HERE", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Chưa cài đặt cấu hình Email server.
        /// </summary>
        public static string LB_LINK_TO_CONFIG_EMAIL_SERVER {
            get {
                return ResourceManager.GetString("LB_LINK_TO_CONFIG_EMAIL_SERVER", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Địa chỉ email nhận chưa được cài đặt.
        /// </summary>
        public static string LB_LINK_TO_CONFIG_EMAIL_SETTINGS_INCOMING {
            get {
                return ResourceManager.GetString("LB_LINK_TO_CONFIG_EMAIL_SETTINGS_INCOMING", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Địa chỉ email gửi chưa được cài đặt.
        /// </summary>
        public static string LB_LINK_TO_CONFIG_EMAIL_SETTINGS_OUTGOING {
            get {
                return ResourceManager.GetString("LB_LINK_TO_CONFIG_EMAIL_SETTINGS_OUTGOING", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Vui lòng click vào Link xác nhận ở trên để tiến hành khôi phục mật khẩu.
        /// </summary>
        public static string LB_RECOVER_PASSWORD_DES {
            get {
                return ResourceManager.GetString("LB_RECOVER_PASSWORD_DES", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Một email đã được gửi đến địa chỉ email [{0}] của bạn. Vui lòng kiểm tra email để thực hiện khôi phục mật khẩu.
        /// </summary>
        public static string NOTIF_EMAIL_FORGOT_PWD_SENT_FORMAT {
            get {
                return ResourceManager.GetString("NOTIF_EMAIL_FORGOT_PWD_SENT_FORMAT", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Một email đã được gửi đến địa chỉ email [{0}] của bạn. Vui lòng kiểm tra email để thực hiện kích hoạt tài khoản.
        /// </summary>
        public static string NOTIF_EMAIL_SENT_FORMAT {
            get {
                return ResourceManager.GetString("NOTIF_EMAIL_SENT_FORMAT", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Quá trình đăng ký thành công, chúng tôi sẽ gửi email kích hoạt tài khoản tới địa chỉ {0} trong vòng 2 ngày làm việc.
        /// </summary>
        public static string NOTIF_REGISTER_AWAITING_FORMAT {
            get {
                return ResourceManager.GetString("NOTIF_REGISTER_AWAITING_FORMAT", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cảm ơn bạn đã đăng ký sử dụng hệ thống.
        /// </summary>
        public static string SUBJECT_AGENCY_REGISTER_THANK {
            get {
                return ResourceManager.GetString("SUBJECT_AGENCY_REGISTER_THANK", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Khôi phục mật khẩu.
        /// </summary>
        public static string SUBJECT_RECOVER_PASSWORD {
            get {
                return ResourceManager.GetString("SUBJECT_RECOVER_PASSWORD", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Xác thực email đăng ký.
        /// </summary>
        public static string SUBJECT_VERIFY_REGISTER {
            get {
                return ResourceManager.GetString("SUBJECT_VERIFY_REGISTER", resourceCulture);
            }
        }
    }
}