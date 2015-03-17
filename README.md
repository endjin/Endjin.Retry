Endjin.Retry
============

Endjin.Retry is API-compatible with Task<T>.Factory.StartNew()

We provide two additional parameters, an IRetryPolicy and an IRetryStrategy.

Strategies
==========

Count: will retry immediately a number of times

Incremental: retries a number of times, with an (optionally increasing) delay between
retries.

Backoff: is similar to Incremental, but provides an exponentially increasing  delay between retries, with a random element.

Note - you don't want to use the Incremental or BackOff strategies in Windows Azure. It is better to hammer the fabric and let it adapt to your preferred usage pattern.

Policies
========

Policies are used to determine whether we can retry given that a particular exception has occurred
The default policy is AnyException - you can always retry regardless of the particular exception or its content

We also provide an AggregatePolicy which allows you to retry if and only if all of a set of policies allow you to retry. 

It is up to you to write custom policy if you want particular exceptions to be "non-retryable" For example, you might set up a policy that does not allow you to retry if you get a 404 (not found) from an http operation, but does retry if you get a 501 (internal server error)

See http://blogs.endjin.com/2013/05/retrying-tasks-with-tpl-async-and-synchronous-code/ for more examples.

If you would like similar functionality from PowerShell, see the following blog post: http://blogs.endjin.com/2014/07/how-to-retry-commands-in-powershell/
