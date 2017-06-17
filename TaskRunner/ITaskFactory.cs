﻿using MooPromise.ThreadPool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise.TaskRunner
{
    internal interface ITaskFactory : IDisposable
    {
        ITaskResult Create(Action action);
        ITaskResult Create(Func<ITaskResult> action);
        ITaskResult Create(Action action, int priority);
        ITaskResult Create(Func<ITaskResult> action, int priority);
        ITaskResult Create(Func<object> action);
        ITaskResult Create(Func<object> action, int priority);
        ITaskResult CreateImmediately(Action action);
        ITaskResult CreateImmediately(Func<ITaskResult> action);
        ITaskResult CreateImmediately(Func<object> action);
        ITaskResult Begin(Action action);
        ITaskResult Begin(Action action, int priority);
        ITaskResult BeginImmediately(Action action);
        ITaskResult Begin(Func<ITaskResult> action);
        ITaskResult Begin(Func<ITaskResult> action, int priority);
        ITaskResult BeginImmediately(Func<ITaskResult> action);
        IThreadPool ThreadPool { get; }
    }
}
