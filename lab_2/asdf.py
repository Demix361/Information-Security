'''

s = 4
a = [0, 0, 0]

for i in range(64):
	print(a)
	for j in range(3):
		if a[j] == s - 1:
			a[j] = -1
			a[j] += 1
		else:
			a[j] += 1
			break
'''

a = [45, 2, 67, 23, 11]

for i in range(50):
	print(a[2 - (i % len(a))])