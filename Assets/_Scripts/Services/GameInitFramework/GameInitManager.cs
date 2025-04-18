using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Cysharp.Threading.Tasks;
using Zenject;

namespace Services.GameInitFramework
{
    public class GameInitManager : IInitializable, IDisposable
    {
        private DiContainer _container;
        private IRequireInit[] _initSources;
        private Dictionary<Type, IList> _initializablesBySource;
        private IInitializableAfterAll[] _afterAllInitializables;

        public GameInitManager(DiContainer container,
            [Inject(Optional = true, Source = InjectSources.Any)]
            IRequireInit[] initSources,
            [Inject(Optional = true, Source = InjectSources.Local)]
            IInitializableAfterAll[] afterAllInitializables)
        {
            _container = container;
            _initSources = initSources;
            _afterAllInitializables = afterAllInitializables;
        }

        public void Initialize()
        {
            FindInitializables();
            WaitForAll().Forget();
        }

        public void Dispose()
        {
            _container = null;
            _initSources = null;
            _afterAllInitializables = null;
            _initializablesBySource?.Clear();
        }

        private void FindInitializables()
        {
            _initializablesBySource = new();
            foreach (var source in _initSources)
            {
                var sourceType = source.GetType();
                var interfaceType = typeof(IInitializableAfter<>).MakeGenericType(sourceType);
                var context = new InjectContext(_container, interfaceType);
                context.Optional = true;
                context.SourceType = InjectSources.Local;
                _initializablesBySource[sourceType] = _container.ResolveAll(context);
            }
        }

        private async UniTask WaitForAll()
        {
            var tasks = new UniTask[_initSources.Length];
            for (var i = 0; i < _initSources.Length; i++)
            {
                tasks[i] = WaitForInit(_initSources[i]);
            }

            await UniTask.WhenAll(tasks);

            await UniTask.Yield();
            foreach (var initializable in _afterAllInitializables)
                initializable.OnAllInitFinished();
        }

        private async UniTask WaitForInit(IRequireInit source)
        {
            if (!source.InitFinished)
                await UniTask.WaitUntil(() => source.InitFinished);
            if (!_initializablesBySource.TryGetValue(source.GetType(), out var initializables))
                return;
            await UniTask.Yield();
            var interfaceType = typeof(IInitializableAfter<>).MakeGenericType(source.GetType());
            var methodName =
                nameof(IInitializableAfter<IRequireInit>.OnInitFinishedFor); // Taking data Manager as an example
            var method = interfaceType.GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public)!;
            foreach (var initializable in initializables)
                if (interfaceType.IsInstanceOfType(initializable))
                    method.Invoke(initializable, new object[] { source });
        }
    }
}