// Copyright (c) 2025 Car Wash Processor, All Rights Reserved.

namespace CarWashProcessor.Application.Abstractions.Resolution
{
    public interface IServiceResolver<TKey, TService>
    {
        TService Resolve(TKey key);
    }
}
