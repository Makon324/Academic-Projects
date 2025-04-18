import torch
from torchvision import transforms
from torch.utils.data import Dataset
from torch.utils.data import DataLoader
import torch.nn as nn
from PIL import Image

torch.set_printoptions(sci_mode=False)


transform = transforms.Compose([
    transforms.Resize((125, 100)),
    transforms.Grayscale(num_output_channels=1),
    transforms.ToTensor(),           # Convert image to PyTorch tensor
])

class SimpleNN(nn.Module):
    def __init__(self):
        super(SimpleNN, self).__init__()
        self.fc1 = nn.Linear(12500, 1000)  # Input size: 784, Output size: 128
        self.sigmoid1 = nn.Sigmoid()
        self.fc2 = nn.Linear(1000, 128)
        self.sigmoid2 = nn.Sigmoid()
        self.fc3 = nn.Linear(128, 1)    # Input size: 128, Output size: 10 (for 10 classes)
        self.sigmoid3 = nn.Sigmoid()

    def forward(self, x):
        x = self.fc1(x)
        x = self.sigmoid1(x)
        x = self.fc2(x)
        x = self.sigmoid2(x)
        x = self.fc3(x)
        x = self.sigmoid3(x)
        return x



model_path = r"model1.pt"

model = SimpleNN()
model.load_state_dict(torch.load(model_path))

model.eval()

print("ready")
while True:
    path = input()
    try:
            print('{:.20f}'.format(model(transform(Image.open(path).convert('RGB')).view(-1))[0].item()))
    except:
        print("havent found any image with that path")
        print()