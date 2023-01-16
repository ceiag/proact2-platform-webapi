using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Proact.Services.Services;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Proact.Services.QueriesServices {
    public class ConsistencyRulesHelper {
        private bool _checkIsOk = true;
        private IActionResult _returnObject;
        private ObjectResult _objectResult;
        private readonly IServiceProvider _serviceProvider;
        private readonly IStringLocalizer<Resource> _stringLocalizer;

        public T GetQueriesService<T>() where T : IQueriesService {
            return (T)_serviceProvider.GetService( typeof( T ) );
        }

        public IStringLocalizer StringLocalizer {
            get => _stringLocalizer;
        }

        public ConsistencyRulesHelper( 
            IServiceProvider serviceProvider, IStringLocalizer<Resource> stringLocalizer ) {
            _serviceProvider = serviceProvider;
            _stringLocalizer = stringLocalizer;
        }

        public ConsistencyRulesHelper CheckIf( 
            Func<bool> onCheckResultFunc, 
            Func<ObjectResult> onOkResultFunc, 
            Func<ObjectResult> onFailResultFunc ) {
            
            if ( _checkIsOk ) {
                bool isQueryOk = onCheckResultFunc.Invoke();

                if ( isQueryOk ) {
                    _objectResult = onOkResultFunc.Invoke();
                }
                else {
                    _objectResult = onFailResultFunc.Invoke();
                    _checkIsOk = false;
                }
            }

            return this;
        }

        public ConsistencyRulesHelper SetQueryResult( ObjectResult queryResult ) {
            _objectResult = queryResult;

            if ( _objectResult.StatusCode != (int)HttpStatusCode.OK ) {
                _checkIsOk = false;
            }

            return this;
        }

        public ConsistencyRulesHelper Then( Func<Task<IActionResult>> func ) {
            if ( _checkIsOk ) {
                _returnObject = func.Invoke().Result;
            }

            return this;
        }

        public ConsistencyRulesHelper Then( Func<IActionResult> func ) {
            if ( _checkIsOk ) {
                _returnObject = func.Invoke();
            }

            return this;
        }

        public IActionResult ReturnResult() {
            if ( _checkIsOk ) {
                return _returnObject;
            }

            return _objectResult;
        }
    }
}
