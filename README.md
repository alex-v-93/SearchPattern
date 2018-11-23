# SearchPattern
In the "searchpattern" library is implemented a pattern search.
The prefix tree is selected as the data storage structure, since allows you to find a word in a stable period of time.
Search string matches wildcard:
'*' - symbols count is null or infinity;
'?' - one symbol.

Example: 
1. query - s?h*l; result - school;
2. query - h??e; result - home;
3. query - *ket; result - market.

Also for the demonstration of work implemented application SearchPatternDemo
