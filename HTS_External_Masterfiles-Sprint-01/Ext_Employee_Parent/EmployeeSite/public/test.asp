Testing html
<br /><br />
<% 
    response.write("Test From asp code") 
    response.write("<br> Displaying date... " & date()) 
    dim a, b, c
    a=1
    b=2
    c = a+b
    response.write("<br> Value after add... " & c) 


%>