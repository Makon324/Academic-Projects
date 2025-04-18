import torch
from torchvision import transforms
from torch.utils.data import Dataset
from torch.utils.data import DataLoader
import torch.nn as nn
from PIL import Image
import os

torch.set_printoptions(sci_mode=False)


transform = transforms.Compose([
    transforms.Resize((125, 100)),
    transforms.Grayscale(num_output_channels=1),
    transforms.ToTensor(),           # Convert image to PyTorch tensor
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
                self.data.append(image.view(-1))

    def __len__(self):
        return len(self.data)

    def __getitem__(self, index):
        x = self.data[index]
        y = self.targets[index]
        return x, y


class SimpleNN(nn.Module):
    def __init__(self):
        super(SimpleNN, self).__init__()
        self.fc1 = nn.Linear(12500, 1000)
        self.sigmoid1 = nn.Sigmoid()
        self.fc2 = nn.Linear(1000, 128)
        self.sigmoid2 = nn.Sigmoid()
        self.fc3 = nn.Linear(128, 1)
        self.sigmoid3 = nn.Sigmoid()

    def forward(self, x):
        x = self.fc1(x)
        x = self.sigmoid1(x)
        x = self.fc2(x)
        x = self.sigmoid2(x)
        x = self.fc3(x)
        x = self.sigmoid3(x)
        return x




dataset = CustomDataset(r"dataset")


batch_size = 1
shuffle = True
num_epochs = 50

dataloader = DataLoader(dataset, batch_size=batch_size, shuffle=shuffle)

model = SimpleNN()

criterion = nn.BCELoss()
optimizer = torch.optim.Adam(model.parameters(), lr=0.001)

print("START")
i = 0
for epoch in range(num_epochs):
    for images, labels in dataloader:
        optimizer.zero_grad()
        outputs = model(images)
        labels = labels.unsqueeze(1).float()
        loss = criterion(outputs, labels)
        loss.backward()
        optimizer.step()
        
    print(f'epoch: {i} ended')
    i += 1

torch.save(model.state_dict(), r"model1.pt")




