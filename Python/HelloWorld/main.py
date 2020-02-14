class Program:
    def getStr(self) -> str:
        return "Hi"
    
    def Main(self) -> None:
        text = self.getStr()
        print(text)
        
    print("Hello")

program = Program()
program.Main()