from random import sample


class Rotor:
    def __init__(self, alphabet):
        self.alphabet = alphabet
        self.pos = 0
        self.symbols = sample(self.alphabet, k=len(self.alphabet))


class Deflector:
    def __init__(self, alphabet):
        self.alphabet = alphabet
        self.symbols = [-1 for i in range(len(self.alphabet))]
        self.place_symbols()

    # change
    def place_symbols(self):
        for i in range(len(self.alphabet) - 1, -1, -1):
            self.symbols[i] = self.alphabet[len(self.alphabet) - 1 - i]


def encode(rotors, deflector, message):
    for rotor in rotors:
        



def main():
    alphabet = [i for i in range(20)]

    message = [2, 2, 44, 3, 32, 45, 230, 1, 193]

    rotor_1 = Rotor(alphabet)
    rotor_2 = Rotor(alphabet)
    rotor_3 = Rotor(alphabet)
    deflector = Deflector(alphabet)

    rotors = [rotor_1, rotor_2, rotor_3]

    print(deflector.symbols)





if __name__ == '__main__':
    main()

