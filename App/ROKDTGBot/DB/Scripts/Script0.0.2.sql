alter table public.user 
add column if not exists number int not null default 0;    

with a as (
	select row_number() over(order by chat_id)as _num, id  from public.user 
)
update public.user s set number = a._num
from a where s.id =  a.id;