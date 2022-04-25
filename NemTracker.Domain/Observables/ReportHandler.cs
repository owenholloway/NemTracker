using System;
using System.Collections.Generic;
using NemTracker.Dtos.Reports;

namespace NemTracker.Model.Observables;

public class ReportHandler : IObservable<ReportDto>
{

    private List<IObserver<ReportDto>> _observers;

    public ReportHandler()
    {
        _observers = new List<IObserver<ReportDto>>();
    }

    public IDisposable Subscribe(IObserver<ReportDto> observer)
    {
        
        if (!_observers.Contains(observer))
        { 
            _observers.Add(observer);
        }

        return new Unsubscriber<ReportDto>(_observers, observer);

    }

    public void ReportToBeConsumed(ReportDto reportDto)
    {
        foreach (var observer in _observers)
        {
            observer.OnNext(reportDto);
        }
    }
    
}