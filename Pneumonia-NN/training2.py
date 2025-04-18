import torch
from torchvision import transforms
from torch.utils.data import Dataset
from torch.utils.data import DataLoader
import torch.nn as nn
from PIL import Image
import os

torch.set_printoptions(sci_mode=False)

model_path = r"model2.pt"

transform = transforms.Compose([
    transforms.Resize((125, 100)),
    transforms.Grayscale(num_output_channels=1),
    transforms.ToTensor(),  # Convert image to PyTorch tensor
])


class CustomDataset(Dataset):
    def __init__(self, path_to_images):
        self.data = []
        self.targets = []
        files = os.listdir(path_to_images)
        for file in files:
            if file[-4:] == ".png":
                if file[0] == 'N':
                    self.targets.append(torch.tensor(0))
                else:
                    self.targets.append(torch.tensor(1))

                image = transform(Image.open(path_to_images + '\\' + file).convert('RGB'))
                self.data.append(image)

    def __len__(self):
        return len(self.data)

    def __getitem__(self, index):
        x = self.data[index]
        y = self.targets[index]
        return x, y


class SimpleNN(nn.Module):
    def __init__(self):
        super(SimpleNN, self).__init__()
        self.conv1 = nn.Conv2d(in_channels=1, out_channels=4, kernel_size=3, stride=1, padding=1)
        self.fc1 = nn.Linear(4 * 125 * 100, 2000)
        self.act1 = nn.ReLU()
        self.fc2 = nn.Linear(2000, 1000)
        self.act2 = nn.ReLU()
        self.fc3 = nn.Linear(1000, 1000)
        self.act3 = nn.ReLU()
        self.fc4 = nn.Linear(1000, 128)
        self.act4 = nn.ReLU()
        self.fc5 = nn.Linear(128, 1)
        self.act5 = nn.Sigmoid()

    def forward(self, x):
        print(x.size())
        x = self.conv1(x)
        x = x.view(x.size(0), -1)
        x = self.fc1(x)
        x = self.act1(x)
        x = self.fc2(x)
        x = self.act2(x)
        x = self.fc3(x)
        x = self.act3(x)
        x = self.fc4(x)
        x = self.act4(x)
        x = self.fc5(x)
        x = self.act5(x)
        return x




batch_size = 64
num_epochs = 50


dataset = CustomDataset(r"dataset")

xy, jdsjjsd = dataset.__getitem__(10)
print(xy.size())

conv1 = nn.Conv2d(in_channels=1, out_channels=4, kernel_size=3, stride=1, padding=1)
print(conv1(xy))


dataloader = DataLoader(dataset, batch_size = batch_size, shuffle = True)

for xz, sdfgasf in dataloader:
    print(xz.size())
    print(conv1(xz))
    break

model = SimpleNN()

criterion = nn.BCELoss()
optimizer = torch.optim.Adam(model.parameters(), lr=0.001)

print("START")

for epoch in range(num_epochs):
    model.train()
    for images, labels in dataloader:
        print(images.size())
        optimizer.zero_grad()
        outputs = model(images)
        labels = labels.unsqueeze(1).float()
        loss = criterion(outputs, labels)
        loss.backward()
        optimizer.step()

    print(f"epoch: {epoch} ended", end='')

torch.save(model.state_dict(), model_path)


