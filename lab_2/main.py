from random import sample, randint


class Rotor:
    def __init__(self, alphabet):
        self.alphabet = alphabet
        self.pos = 0
        self.symbols = sample(self.alphabet, k=len(self.alphabet))

    def forward(self, symbol):
        return self.symbols[self.alphabet.index(symbol) - self.pos]

    def backward(self, symbol):
        return (self.symbols.index(symbol) + self.pos) % len(self.alphabet)

    def roll(self):
        self.pos += 1
        if self.pos == len(self.alphabet):
            self.pos = 0


class Deflector:
    def __init__(self, alphabet):
        self.alphabet = alphabet
        self.symbols = [-1 for i in range(len(self.alphabet))]
        self.place_symbols()

    # change
    def place_symbols(self):
        for i in range(len(self.alphabet) - 1, -1, -1):
            self.symbols[i] = self.alphabet[len(self.alphabet) - 1 - i]

    def process(self, symbol):
        return self.symbols[self.alphabet.index(symbol)]


def encode(rotors, deflector, message):
    encrypted_message = []
    s = len(rotors[0].alphabet)

    for sym in message:
        print(f'[{sym}]', end=' -> ')

        for rotor in rotors:
            sym = rotor.forward(sym)
            print(f'{sym}', end=' -> ')

        sym = deflector.process(sym)
        print(f'| {sym}', end=' --> ')

        for i in range(len(rotors) - 1, -1, -1):
            sym = rotors[i].backward(sym)
            if i != 0:
                print(f'{sym}', end=' -> ')
            else:
                print(f'{sym}')

        for j in range(len(rotors)):
            if rotors[j].pos == s - 1:
                rotors[j].pos = 0
            else:
                rotors[j].pos += 1
                break

        encrypted_message.append(sym)

    return encrypted_message


def main():
    alph_size = 26  # 10
    alphabet = [i for i in range(alph_size)]

    #message = [3, 4, 8, 5, 0]
    #message = [6, 7, 5, 0, 7]
    message = [randint(0, alph_size - 1) for i in range(50)]


    rotor_1 = Rotor(alphabet)
    rotor_2 = Rotor(alphabet)
    rotor_3 = Rotor(alphabet)
    # rotor_1.symbols = [9, 0, 1, 2, 3, 4, 5, 6, 7, 8]
    # rotor_2.symbols = [4, 5, 6, 7, 8, 9, 0, 1, 2, 3]
    # rotor_3.symbols = [7, 8, 9, 0, 1, 2, 3, 4, 5, 6]
    deflector = Deflector(alphabet)

    rotors = [rotor_1, rotor_2, rotor_3]
    print(rotor_1.symbols)
    print(rotor_2.symbols)
    print(rotor_3.symbols)


    print()
    print(message)

    new_mes = encode(rotors, deflector, message)

    print(new_mes)





if __name__ == '__main__':
    main()

