str = """
0	13	2	8	4	6	15	11	1	10	9	3	14	5	0	12	7	
1	1	15	13	8	10	3	7	4	12	5	6	11	0	14	9	2
2	7	11	4	1	9	12	14	2	0	6	10	13	15	3	5	8	
3	2	1	14	7	4	10	8	13	15	12	9	0	3	5	6	11				
"""

a = str.split()
a.pop(0)
a.pop(16)
a.pop(32)
a.pop(48)

cs_str = "{\n"
for i in range(4):
    cs_str += " { "
    
    for j in range(16):
        if j != 15:
            cs_str += a[i * 16 + j] + ", "
        else:
            cs_str += a[i * 16 + j] + " "
            
    if i != 3:
        cs_str += "},\n"
    else:
        cs_str += "}\n"
        
cs_str += "};"
print(cs_str)
