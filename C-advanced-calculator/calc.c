#include<stdio.h>
#include<stdlib.h>
#include <stdbool.h>

//l - liczba  s - system liczbowy  z - operator  n - znak nowej lini  N - miejsce na wpisanie liczby  m - inny znak konczoncy  i - inny znak do zignorowania  j - do zignorowania  e - EOF
struct blok{
	char co;  //l - liczba  z - operator  s - system liczbowy  p - miejsce na wypelnienie  e - EOF
	int dlugosc;  //dlugosc danej rzeczy, np. liczba 21024718924 bedzie mia³a dlugosc 11
};

unsigned char czyblad=0;  //0 - brak b³edu, 1 - blad dotyczacy tylko konkretnej czesci, 2 - blad dotyczacy calego przykladu
char bledy[100];
int rb=0;

//funkcje

//ogólne
void swap(unsigned char* x1, unsigned char* x2){
	unsigned char p;
	p = *x1;
	*x1=*x2;
	*x2=p;
}

void zamienianie_kolejnosci(unsigned char* a, int rozmiar){
	for(int i=0; i<rozmiar/2; i++){
		swap((a+i), (a+rozmiar-i-1));
	}
}

int max(int x1, int x2){
	if(x1>x2){return x1;}
	else{return x2;}
}

int min(int x1, int x2){
	if(x1<x2){return x1;}
	else{return x2;}
}

void usuwanie_zer(unsigned char** x, int* rozmiar, int* rozmiar_buffora){
	while(*(*x+*rozmiar-1)==0&&*rozmiar>1){
		*rozmiar-=1;
	}
}

void przesuniecie(unsigned char* x, int* rozmiar){  //takie jak przesuniêcie bitowe, obciecie ostatniej cyfry i przesuniecie reszty
	for(int i=0; i<*rozmiar; i++){
		*(x+i)=*(x+i+1);
	}
	*rozmiar-=1;
}

void charcpy(char text[]){
	int i=0;
	while(i<rb&&i<100){
		bledy[i]=text[i];
		i++;
	}
}

//dodawanie
void dodawanie(unsigned char** x1, int* rozmiar1, int* rozmiar_buffora1, unsigned char* x2, int rozmiar2, unsigned char systemliczbowy){ //bierze 2 argumenty, dodaje je i wynik zapisuje w pierszym, aby móc zapisaæ wynik pierwszy argument przekazywany jest przez refernecjê, wynik i argumenty podawane s¹ i zapisywane odwrotnie
	int rozmiar1org = *rozmiar1;
	*rozmiar1 = max(*rozmiar1, rozmiar2);
	*rozmiar_buffora1 = *rozmiar1+1;
	*x1 = (unsigned char*) realloc(*x1, *rozmiar_buffora1);
	if(*x1==NULL&&czyblad==0){czyblad=2; rb=71; charcpy("b³¹d przy alokacji pamieci, byæ mo¿e liczba, któr¹ poda³eœ jest za du¿a"); return;}
	unsigned char carry=0;
	for(int i=0; i<rozmiar1org||i<rozmiar2; i++){
		if(i>=rozmiar1org){*(*x1+i)=0;}
		if(i<rozmiar2){*(*x1+i)+= *(x2+i);}
		*(*x1+i)+= carry; 
		carry = *(*x1+i)/systemliczbowy;
		*(*x1+i)%=systemliczbowy;
	}
	if(carry>0){
		*(*x1+*rozmiar1)=carry;
		*rozmiar1+=1;
	}
}

//mnozenie
void mnozenie(unsigned char** x1, int* rozmiar1, int* rozmiar_buffora1, unsigned char* x2, int rozmiar2, unsigned char systemliczbowy){ //bierze 2 argumenty, mno¿y je i wynik zapisuje w pierszym, aby móc zapisaæ wynik pierwszy argument przekazywany jest przez refernecjê, wynik i argumenty podawane s¹ i zapisywane odwrotnie
	int rozmiar_bufforaw=1;
	unsigned char* w;
	w = (unsigned char*) malloc(rozmiar_bufforaw);	//w - wynik
	if(w==NULL&&czyblad==0){czyblad=2; rb=71; charcpy("b³¹d przy alokacji pamieci, byæ mo¿e liczba, któr¹ poda³eœ jest za du¿a"); return;}
	*w=0;
	int rozmiarw=1;
	for(int j=0; j<rozmiar2; j++){
		unsigned char carry=0;
		for(int i=0; i<*rozmiar1; i++){
			if(rozmiarw<=i+j){
				rozmiarw++;
				if(rozmiarw>rozmiar_bufforaw){
					rozmiar_bufforaw*=2;
					w = (unsigned char*) realloc(w, rozmiar_bufforaw);
					if(w==NULL&&czyblad==0){czyblad=2; rb=71; charcpy("b³¹d przy alokacji pamieci, byæ mo¿e liczba, któr¹ poda³eœ jest za du¿a"); return;}
				}
				*(w+rozmiarw-1)=0;
			}
			*(w+i+j) += (*(*x1+i))*(*(x2+j))+carry;
			carry = *(w+i+j)/systemliczbowy;
			*(w+i+j)%=systemliczbowy;
		}
		//przepisywanie z carry do wyniku
		if(carry!=0){
			rozmiarw++;
			if(rozmiarw>rozmiar_bufforaw){
				rozmiar_bufforaw*=2;
				w = (unsigned char*) realloc(w, rozmiar_bufforaw);
				if(w==NULL&&czyblad==0){czyblad=2; rb=71; charcpy("b³¹d przy alokacji pamieci, byæ mo¿e liczba, któr¹ poda³eœ jest za du¿a"); return;}
			}
			*(w+rozmiarw-1)=carry;
		}
	}
	free(*x1);
	//przepisywanie wyniku do x1
	*rozmiar_buffora1=rozmiar_bufforaw;
	*rozmiar1=rozmiarw;
	*x1 = (unsigned char*) malloc(*rozmiar_buffora1);
	if(*x1==NULL&&czyblad==0){czyblad=2; rb=71; charcpy("b³¹d przy alokacji pamieci, byæ mo¿e liczba, któr¹ poda³eœ jest za du¿a"); return; /*printf("b³¹d przy alokacji pamiêci"); exit(1);*/}
	for(int i=0; i<rozmiarw; i++){
		*(*x1+i) = *(w+i);
	}
	free(w);
}

//dzielenie
void odejmowanie(unsigned char* x1, int rozmiar1, unsigned char* x2, int rozmiar2, unsigned char systemliczbowy){ //bierze 2 argumenty, odejmuje je i wynik zapisuje w pierszym, aby móc zapisaæ wynik pierwszy argument przekazywany jest przez refernecjê, wynik i argumenty podawane s¹ i zapisywane odwrotnie, dzia³a póki x1>x2
	for(int i=0; i<rozmiar2; i++){
		if(*(x1+i)<*(x2+i)||*(x1+i)>systemliczbowy){ //  "po¿yczanie"
			*(x1+i)+=systemliczbowy;
			*(x1+i+1)-=1;
		}
		*(x1+i)-=*(x2+i);
	}
}

bool porow(unsigned char* x1, int rozmiar1, unsigned char* x2, int rozmiar2){ //0  -  <=	1  -  >
	//x1, x2	-	odwrocone
	bool w=0;
	int rozmin = min(rozmiar1, rozmiar2);
	signed char czy=0;
	if(rozmiar1>rozmiar2){czy=1;}
	if(rozmiar1<rozmiar2){czy=-1;}
	for(int i=rozmin-1; i>=0; i--){
		if(*(x1+i)<*(x2+i)){w=0; break;}
		if(*(x1+i)>*(x2+i)){w=1; break;}
	}
	if(czy==1){
		for(int i=rozmiar2; i<rozmiar1; i++){
			if(*(x1+i)>0){w=1;}
		}
	}
	if(czy==-1){
		for(int i=rozmiar1; i<rozmiar2; i++){
			if(*(x2+i)>0){w=0;}
		}
	}
	return w;
}

unsigned char ile_razy_sie_miesci(unsigned char* x1, int rozmiar1, unsigned char* x2, int rozmiar2, unsigned char systemliczbowy){ //ile razy x2 miesci sie w x1
	unsigned char* w;
	int rozmiar_bufforaw=1;
	int rozmiarw=1;
	w = (unsigned char*) malloc(rozmiar_bufforaw); *w=0;
	unsigned char ile=0;
	if(porow(w, rozmiarw, x1, rozmiar1)==1){return ile;} //gdy x2 = 0
	while(1){
		if(porow(w, rozmiarw, x1, rozmiar1)==1){return ile-1;}
		dodawanie(&w, &rozmiarw, &rozmiar_bufforaw, x2, rozmiar2, systemliczbowy);
		ile++;
	}
	free(w);
}

void reszta_z_ile_sie_miesci(unsigned char* x1, int rozmiar1, unsigned char* x2, int rozmiar2, unsigned char systemliczbowy, unsigned char ile_razy_sie_miesci){ //x1%=x2
	for(unsigned char i=0; i<ile_razy_sie_miesci; i++){
		odejmowanie(x1, rozmiar1, x2, rozmiar2, systemliczbowy);
	}
}

void reszta_z_dzielenia(unsigned char** x1, int* rozmiar1, int* rozmiar_buffora1, unsigned char* x2, int rozmiar2, unsigned char systemliczbowy){ //bierze 2 argumenty, dzieli je i resztê z dzielenia zapisuje w pierszym, aby móc zapisaæ wynik pierwszy argument przekazywany jest przez refernecjê, wynik i argumenty podawane s¹ i zapisywane odwrotnie(x1%=x2)
	zamienianie_kolejnosci(*x1, *rozmiar1);
	for(int i=0; i<*rozmiar1; i++){ 
		zamienianie_kolejnosci(*x1, i+1);
		unsigned char ile=ile_razy_sie_miesci(*x1, i+1, x2, rozmiar2, systemliczbowy);
		reszta_z_ile_sie_miesci(*x1, i+1, x2, rozmiar2, systemliczbowy, ile);
		zamienianie_kolejnosci(*x1, i+1);
	}
	zamienianie_kolejnosci(*x1, *rozmiar1);
	usuwanie_zer(x1, rozmiar1, rozmiar_buffora1);
}

void dzielenie(unsigned char** x1, int* rozmiar1, int* rozmiar_buffora1, unsigned char* x2, int rozmiar2, unsigned char systemliczbowy){ //bierze 2 argumenty, dzieli je i wynik zapisuje w pierszym, aby móc zapisaæ wynik pierwszy argument przekazywany jest przez refernecjê, wynik i argumenty podawane s¹ i zapisywane odwrotnie (x1/=x2)
	unsigned char* w;
	int rozmiar_bufforaw=1;
	int rozmiarw=0;
	w = (unsigned char*) malloc(rozmiar_bufforaw);
	zamienianie_kolejnosci(*x1, *rozmiar1);
	bool czykoniec0=0;
	for(int i=0; i<*rozmiar1; i++){ 
		zamienianie_kolejnosci(*x1, i+1);
		unsigned char ile=ile_razy_sie_miesci(*x1, i+1, x2, rozmiar2, systemliczbowy);
		reszta_z_ile_sie_miesci(*x1, i+1, x2, rozmiar2, systemliczbowy, ile);
		zamienianie_kolejnosci(*x1, i+1);
		if(czykoniec0==1||ile>0){
			czykoniec0=1;
			rozmiarw++;
			if(rozmiarw>rozmiar_bufforaw){
				rozmiar_bufforaw*=2;
				w = (unsigned char*) realloc(w, rozmiar_bufforaw);
				if(w==NULL){czyblad=2; rb=71; charcpy("b³¹d przy alokacji pamieci, byæ mo¿e liczba, któr¹ poda³eœ jest za du¿a"); return;}
			}
			*(w+rozmiarw-1)=ile;
		}
	}
	if(rozmiarw==0){ //gdy wynikiem dzielenia jest 0
		rozmiarw++;
		*w=0;
	}
	zamienianie_kolejnosci(w, rozmiarw);  
	free(*x1);			//przepisywanie wyniku do x1
	*x1 = w;
	*rozmiar1=rozmiarw;
	*rozmiar_buffora1=rozmiar_bufforaw;
}

//potegowanie
unsigned char mod2(unsigned char* x, int rozmiar, unsigned char systemliczbowy){  //sprawdza podzielnoœæ przez 2 liczby argumentu, argument podawany jest odwrotnie
	if(systemliczbowy%2==0){
		if(*x%2==0){return 0;}
		else{return 1;}
	}
	unsigned char w=0;
	for(int i=0; i<rozmiar; i++){
		w = (w+*(x+i))%2;
	}
	return w;
}

void dzielenie_przez_2(unsigned char** x, int* rozmiar, int* rozmiar_buffora, unsigned char systemliczbowy){  //dzieli argument przez 2, argument podawany jest odwrotnie przez referencjê
	if(systemliczbowy==2){przesuniecie(*x, rozmiar); return;}
//dzielenie
	unsigned char* w;
	int rozmiar_bufforaw=1;
	int rozmiarw=0;
	w = (unsigned char*) malloc(rozmiar_bufforaw);
	zamienianie_kolejnosci(*x, *rozmiar);
	bool czykoniec0=0;
	for(int i=0; i<*rozmiar; i++){
		unsigned char ile;
		if(i>0){*(*x+i)=*(*x+i)+*(*x+i-1)*systemliczbowy;}  //mod 2 zawsze tylko 1 lub 0
		ile=*(*x+i)/2;
		*(*x+i)%=2;
		if(czykoniec0==1||ile>0){
			czykoniec0=1;
			rozmiarw++;
			if(rozmiarw>rozmiar_bufforaw){
				rozmiar_bufforaw*=2;
				w = (unsigned char*) realloc(w, rozmiar_bufforaw);
				if(w==NULL){czyblad=2; rb=71; charcpy("b³¹d przy alokacji pamieci, byæ mo¿e liczba, któr¹ poda³eœ jest za du¿a"); return;}
			}
			*(w+rozmiarw-1)=ile;
		}
	}
	if(rozmiarw==0){ //gdy wynikiem dzielenia jest 0
		rozmiarw++;
		*w=0;
	}
	zamienianie_kolejnosci(w, rozmiarw);
	free(*x);
	*x = w;
	*rozmiar=rozmiarw;
	*rozmiar_buffora=rozmiar_bufforaw;
}

void potegowanie(unsigned char** x1, int* rozmiar1, int* rozmiar_buffora1, unsigned char** x2, int* rozmiar2, int* rozmiar_buffora2, unsigned char systemliczbowy){  //bierze 2 argumenty, potêguje je i wynik zapisuje w pierszym, aby móc edytowaæ argumenty, argumenty przekazywane s¹ przez refernecjê, wynik i argumenty podawane s¹ i zapisywane odwrotnie (x1^x2)
	unsigned char* w;
	int rozmiar_bufforaw=1;
	int rozmiarw=1;
	w = (unsigned char*) malloc(rozmiar_bufforaw); *w=1;
	while(**x2>0||*rozmiar2>1){
		if(mod2(*x2, *rozmiar2, systemliczbowy)==1){
			mnozenie(&w, &rozmiarw, &rozmiar_bufforaw, *x1, *rozmiar1, systemliczbowy);
		}
		unsigned char* duplikat;  //duplikat x2 do mnozenia, nie mozemy przekazac x1 jako drugi argument bo mno¿enie zmienia zawartoœæ x1 w trakcie dzia³ania, nie na rozmiaru_buffora, bo nie zmienia siê on podczas dzia³ania funkcji
		int rozmiard=*rozmiar1;
		duplikat = (unsigned char*) malloc(rozmiard);
		for(int i=0; i<rozmiard; i++){*(duplikat+i)=*(*x1+i);}  //przepisywanie x1 do duplikatu
		mnozenie(x1, rozmiar1, rozmiar_buffora1, duplikat, rozmiard, systemliczbowy);
		free(duplikat);
		dzielenie_przez_2(x2, rozmiar2, rozmiar_buffora2, systemliczbowy);
	}
	free(*x1);		//przepisywanie wyniku do x1
	*x1 = w;
	*rozmiar1=rozmiarw;
	*rozmiar_buffora1=rozmiar_bufforaw;
}

//zamienianie systemów liczbowych
void zapisywanie_w_systemie(unsigned char x, unsigned char* xp, int* rozmaiarxp, unsigned char systemliczbowy){  //zapisuje liczbê x w systemie liczbowym, wynik zapisuje w arrayu przekazanym przez referencjê
	while(x>0){
		*(xp+*rozmaiarxp)=x%systemliczbowy;
		*rozmaiarxp+=1;
		x/=systemliczbowy;
	}
}

void zamienianie_systemow(unsigned char** x1, int* rozmiar1, int* rozmiar_buffora1, unsigned char systemliczbowy1, unsigned char systemliczbowy2){  //zamienia liczbe z systemu 1 na 2
//przedstawianie systemu 1 w systemie 2
	unsigned char* s1ws2;  //system liczbowy 1 w systemie liczbowym 2
	s1ws2 = (unsigned char*) malloc(5);
	int rozmiars1ws2=0;
	zapisywanie_w_systemie(systemliczbowy1, s1ws2, &rozmiars1ws2, systemliczbowy2);
//deklarowanie zmiennej trzymajacej przez co aktualnie mnozymy
	unsigned char* m;
	int rozmiar_bufforam=1;
	int rozmiarm=1;
	m = malloc(rozmiar_bufforam); *m=1;
//deklarowanie zmiennej wyniku
	unsigned char* w;
	int rozmiar_bufforaw=1;
	int rozmiarw=1;
	w = malloc(rozmiar_bufforaw); *w=0;
//zamienianie systemów
	for(int i=0; i<*rozmiar1; i++){
		unsigned char* l;
		int rozmiar_bufforal=5;  //buffor na 5 a nie jakos sprytnie, bo nie ma sensu marnowac paru realloc za kilka bajtów
		int rozmiarl=0;
		l = (unsigned char*) malloc(rozmiar_bufforal);
		zapisywanie_w_systemie(*(*x1+i), l, &rozmiarl, systemliczbowy2);
		mnozenie(&l, &rozmiarl, &rozmiar_bufforal, m, rozmiarm, systemliczbowy2);
		dodawanie(&w, &rozmiarw, &rozmiar_bufforaw, l, rozmiarl, systemliczbowy2);
		free(l);
		mnozenie(&m, &rozmiarm, &rozmiar_bufforam, s1ws2, rozmiars1ws2, systemliczbowy2);
	}
	free(s1ws2);
	free(m);
	free(*x1);
//zapisywanie wyniku w x1
	*rozmiar_buffora1=rozmiar_bufforaw;
	*rozmiar1=rozmiarw;
	*x1 = (unsigned char*) malloc(*rozmiar_buffora1);
	if(*x1==NULL){czyblad=2; rb=71; charcpy("b³¹d przy alokacji pamieci, byæ mo¿e liczba, któr¹ poda³eœ jest za du¿a"); return;}
	for(int i=0; i<rozmiarw; i++){
		*(*x1+i) = *(w+i);
	}
	free(w);
}

//wczytywanie 
unsigned char ascii_to_uchar(char a){ //zamienia znak na liczbê ca³kowit¹ typu unsigned char
	if(a>='0'&&a<='9'){return a-'0';}
	if(a>='A'&&a<='F'){return a-'A'+10;}
	if(a>='a'&&a<='f'){return a-'a'+10;}
	if(czyblad==0){czyblad=1; rb=19; charcpy("nieznana cyfra: '%'"); bledy[17]=a; return 0;}
}

char uchar_to_ascii(unsigned char a){
	if(a>=0&&a<=9){return a+'0';}
	if(a>=10&&a<=15){return a+'A'-10;}
	printf("blad zamiana: %hhu na int", a);
	exit(1);
}

char coteraz(char* plik, int indeks){  //l - liczba  z - operator  n - znak nowej lini  m - inny znak konczoncy  i - inny znak do zignorowania  e - EOF
	char t=*(plik+indeks);
	if((t>='0'&&t<='9')||(t>='A'&&t<='F')||(t>='a'&&t<='f')){return 'l';}
	else if(t=='+'||t=='*'||t=='/'||t=='%'||t=='^'){return 'z';}
	else if(t=='\n'){return 'n';}
	else if(t==' '||t=='\t'){return 'm';}
	else if(t==EOF){return 'e';}
	else{return 'i';}
}

int przechodzenie_po_liczbie(char* plik, int* indeks){  //nie trzeba rozmiaru_pliku, bo konczy sie 'e', zwraca dlugosc liczby
	int dlugosc=1;
	while(coteraz(plik, *indeks)=='l'||coteraz(plik, *indeks)=='i'){
		dlugosc++;
		*indeks+=1;
	}
	return dlugosc;
}

int przechodzenie_po_enterze(char* plik, int* indeks){  //nie trzeba rozmiaru_pliku, bo konczy sie 'e', zwraca dlugosc przerwy
	int dlugosc=1;
	while(coteraz(plik, *indeks)=='n'){
		dlugosc++;
		*indeks+=1;
	}
	return dlugosc;
}

int przechodzenie_po_spacji(char* plik, int* indeks){  //nie trzeba rozmiaru_pliku, bo konczy sie 'e', zwraca dlugosc przerwy
	int dlugosc=1;
	while(coteraz(plik, *indeks)=='m'){
		dlugosc++;
		*indeks+=1;
	}
	return dlugosc;
}

int przechodzenie_po_i(char* plik, int* indeks){  //nie trzeba rozmiaru_pliku, bo konczy sie 'e', zwraca ci¹gu innych znaków
	int dlugosc=1;
	while((coteraz(plik, *indeks)=='i'||(*(plik+*indeks)>='a'&&*(plik+*indeks)<='f')||(*(plik+*indeks)>='A'&&*(plik+*indeks)<='F'))&&coteraz(plik, *indeks)!='e'){
		dlugosc++;
		*indeks+=1;
	}
	return dlugosc;
}

unsigned char szukanie_liczby(struct blok* schemat, int* indeks, int rozmiar, bool* czyjestznak){  //0 - znalaz³ liczbe, 1- znalaz³ znak potem m a potem znowu liczbe, interpretuje to jako nowy przyklad, 2 - znalazl liczbe potem m a potem znowu liczbe, a przed tym miejsce, interpretuje to jako zamiana systemow
	while((schemat+*indeks)->co!='l'&&*indeks<rozmiar){
		if(*indeks>0&&*indeks+2<rozmiar){if((schemat+*indeks-1)->co=='n'&&(schemat+*indeks-1)->dlugosc>=4&&(schemat+*indeks)->co=='l'&&(schemat+*indeks+1)->co=='m'&&(schemat+*indeks+2)->co=='l'&&*czyjestznak==1){
			*czyjestznak=0;
			(schemat+*indeks-1)->co='N';
			(schemat+*indeks)->co='S';
			(schemat+*indeks+2)->co='s';
			return 2;
		}}
		if(*indeks>0&&*indeks+2<rozmiar){if((schemat+*indeks-1)->co=='n'&&(schemat+*indeks-1)->dlugosc>=4&&(schemat+*indeks)->co=='z'&&(schemat+*indeks+1)->co=='m'&&(schemat+*indeks+2)->co=='l'&&*czyjestznak==1){
			*czyjestznak=0;
			(schemat+*indeks-1)->co='N';
			(schemat+*indeks)->co='z';
			(schemat+*indeks+2)->co='s';
			return 1;
		}}
		*indeks+=1;
	}
	return 0;
}

void wczytywanie_pliku_do_tablicy(FILE* file, char** plik, int* rozmiar_pliku, int* rozmiar_buffora_pliku){
	char t=getc(file);
	while(t!=EOF){
		*rozmiar_pliku+=1;
		if(*rozmiar_pliku>=*rozmiar_buffora_pliku){
			*rozmiar_buffora_pliku*=2;
			*plik = (char*) realloc(*plik, *rozmiar_buffora_pliku);
		}
		*(*plik+*rozmiar_pliku-1)=t;
		t=getc(file);
	}
	*(*plik+*rozmiar_pliku)=EOF;
	*rozmiar_pliku+=1;
}

void usuwanie(struct blok** schemat, int* rozmiar_schematu, int* rozmiar_buffora_schematu, int indeks){  //usuwa *(*schemat+indeks)
	for(int i=indeks; i<*rozmiar_schematu-1; i++){
		*(*schemat+i)=*(*schemat+i+1);
	}
	*rozmiar_schematu-=1;
}

void przerabianie_na_schemat(char* plik, struct blok** schemat, int* rozmiar_schematu, int* rozmiar_buffora_schematu){
//poczatkowe przerabianie
	int indeks=0;
	char co=coteraz(plik, indeks);
	while(co!='e'){
		indeks++;
		*rozmiar_schematu+=1;
		if(*rozmiar_schematu>*rozmiar_buffora_schematu){
			*rozmiar_buffora_schematu*=2;
			*schemat = (struct blok*) realloc(*schemat, *rozmiar_buffora_schematu*sizeof(*schemat));
		}
		(*schemat+*rozmiar_schematu-1)->co=co;
		(*schemat+*rozmiar_schematu-1)->dlugosc=1;
		if(co=='i'){(*schemat+*rozmiar_schematu-1)->dlugosc=przechodzenie_po_i(plik, &indeks);}
		if(co=='l'){(*schemat+*rozmiar_schematu-1)->dlugosc=przechodzenie_po_liczbie(plik, &indeks);}
		if(co=='n'){(*schemat+*rozmiar_schematu-1)->dlugosc=przechodzenie_po_enterze(plik, &indeks);}
		if(co=='m'){(*schemat+*rozmiar_schematu-1)->dlugosc=przechodzenie_po_spacji(plik, &indeks);}
		
		co=coteraz(plik, indeks);
	}
		
//zamienianie l na s, l na S, n na N, i na z w odpowiednich miejsach	
	bool czyjestznak=0;  //zapisuje czy juz znalazl znak po ostatnim miejscu na wpisanie liczby
	bool czyjestsystem=0;
	for(int i=0; i<*rozmiar_schematu; i++){
		if((*schemat+i)->co=='z'){
			if(czyjestznak==0){czyjestznak=1; czyjestsystem=0; continue;}
			else{czyjestznak=1; czyjestznak=1; czyjestsystem=0; continue;}
		}
		if((*schemat+i)->co=='l'){
			if(czyjestznak==1){
				if(czyjestsystem==0){(*schemat+i)->co='s'; czyjestsystem=1;}
				else if(i+2<*rozmiar_schematu){if((*schemat+i+1)->co=='m'&&(*schemat+i+2)->co=='l'){
					(*schemat+i)->co='S'; czyjestznak=1; czyjestsystem=0; continue;
				}}
			}
			else{
				(*schemat+i)->co='S'; czyjestznak=1; czyjestsystem=0; continue;
			}
		}
		if((*schemat+i)->co=='n'&&(*schemat+i)->dlugosc>=4&&czyjestznak==1&&czyjestsystem==1){(*schemat+i)->co='N'; continue;}
		if((*schemat+i)->co=='n'&&i+1==*rozmiar_schematu){(*schemat+i)->co='N'; continue;}
		if((*schemat+i)->co=='n'&&(*schemat+i)->dlugosc>=4&&czyjestznak==1&&czyjestsystem==0){
			if(i+1<*rozmiar_schematu){if((*schemat+i+1)->co=='z'){
				(*schemat+i)->co='N'; czyjestznak=1; czyjestsystem=0;
				i++; continue;
			}}
			if(i+3<*rozmiar_schematu){if((*schemat+i+1)->co=='l'&&(*schemat+i+2)->co=='m'&&(*schemat+i+3)->co=='l'){
				(*schemat+i)->co='N'; (*schemat+i+1)->co='S'; (*schemat+i+3)->co='s'; czyjestznak=1; czyjestsystem=1;
				i+=3; continue;
			}}
		}
		if((*schemat+i)->co=='n'&&czyjestznak==1){
			if(i+1<*rozmiar_schematu){if((*schemat+i+1)->co=='z'){
				(*schemat+i)->co='N'; czyjestznak=1; czyjestsystem=0;
				i++; continue;
			}}
			if(i+3<*rozmiar_schematu){if((*schemat+i+1)->co=='l'&&(*schemat+i+2)->co=='m'&&(*schemat+i+3)->co=='l'){
				(*schemat+i)->co='N'; (*schemat+i+1)->co='S'; (*schemat+i+3)->co='s'; czyjestznak=1; czyjestsystem=1;
				i+=3; continue;
			}}
		}
		
		
		if(i>0&&i+1<*rozmiar_schematu){if((*schemat+i)->co=='i'&&czyjestznak==0&&((*schemat+i-1)->co=='m'||(*schemat+i-1)->co=='n')&&((*schemat+i+1)->co=='m'||(*schemat+i+1)->co=='n')){
			(*schemat+i)->co='z'; czyjestznak=1; czyjestsystem=0; continue;
		}}
		if(i==0){if((*schemat+i)->co=='i'&&czyjestznak==0&&((*schemat+i+1)->co=='m'||(*schemat+i+1)->co=='n')){(*schemat+i)->co='z'; czyjestznak=1; czyjestsystem=0; continue;}}
	}
	
//n, m oraz i na j
	for(int i=0; i<*rozmiar_schematu; i++){
		if((*schemat+i)->co=='n'||(*schemat+i)->co=='m'||(*schemat+i)->co=='i'){
			(*schemat+i)->co='j';
		}
	}
	
//scalanie j ze sob¹ - gdy jest j j to zamienia sie to na j o dlugosci sumy tych 2 j
	for(int i=0; i<*rozmiar_schematu-1; i++){
		if((*schemat+i)->co=='j'&&(*schemat+i+1)->co=='j'){
			int dl = (*schemat+i)->dlugosc;
			usuwanie(schemat, rozmiar_schematu, rozmiar_buffora_schematu, i);
			(*schemat+i)->dlugosc+=dl;
		}
	}
	
//gdy ostatni element to nie N, to dodawanie N 0 na koncu
	if((*schemat+*rozmiar_schematu-1)->co!='N'){
		*rozmiar_schematu+=1;
		if(*rozmiar_schematu>*rozmiar_buffora_schematu){
			*rozmiar_buffora_schematu+=1;
			*schemat = (struct blok*) realloc(*schemat, *rozmiar_buffora_schematu*sizeof(*schemat));
		}
		(*schemat+*rozmiar_schematu-1)->co='N';
		(*schemat+*rozmiar_schematu-1)->dlugosc=0;
	}
	
}

void wczytywanie_znaku(char* plik, int indeks, int rozmiar, char* znak){
	*znak=*(plik+indeks);
	if(*znak!='+'&&*znak!='*'&&*znak!='/'&&*znak!='%'&&*znak!='^'&&czyblad==0){czyblad=2; rb=22; charcpy("nieznany operator: '%'"); bledy[20]=*znak; return; /*printf("nieznany operator: '%c'", *znak); exit(1);*/}
}

void wczytywanie_liczby(char* plik, int indeks, int rozmiar_liczby, unsigned char** x, int* rozmiarx, int* rozmiar_bufforax, unsigned char systemliczbowy){
	int i=0;
	while(*(plik+indeks+i)=='0'&&i<rozmiar_liczby){i++;}  //wczytuje liczbe z usunietymi zerami na poczatku
	while(i<rozmiar_liczby){
		*rozmiarx+=1;
		if(*rozmiarx>*rozmiar_bufforax){
			*rozmiar_bufforax*=2;
			*x = (unsigned char*) realloc(*x, *rozmiar_bufforax);
			if(*x==NULL&&czyblad==0){czyblad=2; rb=71; charcpy("b³¹d przy alokacji pamieci, byæ mo¿e liczba, któr¹ poda³eœ jest za du¿a"); return; /*printf("b³¹d przy alokacji pamieci"); exit(1);*/}
		}
		*(*x+*rozmiarx-1)=ascii_to_uchar(*(plik+indeks+i));
		if(*(*x+*rozmiarx-1)>=systemliczbowy&&czyblad==0){czyblad=1; rb=42; charcpy("liczba nie miesci sie w systemie liczbowym"); return; /*printf("liczba nie miesci sie w systemie liczbowym"); exit(1);*/}
		i++;
	}
}

bool wczytywanie_systemu_liczbowego(char* plik, int indeks, int rozmiar_systemu, unsigned char* system){  //0 - powiodlo sie  1 - nie powiodlo sie
	*system=0;
	for(int i=0; i<rozmiar_systemu; i++){
		unsigned char l=*(plik+indeks+i)-'0';
		if((*(plik+indeks+i)<'0'||*(plik+indeks+i)>'9')&&czyblad==0){czyblad=2; rb=19; charcpy("nieznana cyfra: '%'"); bledy[17]=*(plik+indeks+i); return 1; /*printf("nieznana cyfra: %c\n", *(plik+indeks+i)); exit(1);*/}
		*system*=10;
		*system+=l;
	}
	if((*system>16||*system<2)&&czyblad==0){czyblad=2; rb=26; charcpy("nieznany system liczbowy: "); for(int i=0; i<rozmiar_systemu; i++){bledy[26+i]=*(plik+indeks+i); rb++;} return 1; /*printf("nieznany system liczbowy: %i", *system); exit(1);*/}
	return 0;
}

//wypisywanie
void wypisywanie_liczby(FILE* plik, unsigned char* x, int rozmiar, int dlugosc_miejsca){  //wypisuje liczbe do pliku w miejscu gdzie aktualnie jest wskaŸnik
	if(dlugosc_miejsca<4){dlugosc_miejsca=4;}
	for(int i=0; i<2; i++){putc('\n', plik);}
	for(int i=0; i<rozmiar; i++){
		putc(uchar_to_ascii(*(x+i)), plik);
	}
	for(int i=2; i<dlugosc_miejsca; i++){putc('\n', plik);}
}

void wypisywanie_bledu(FILE* plik, int dlugosc_miejsca){
	if(dlugosc_miejsca<4){dlugosc_miejsca=4;}
	for(int i=0; i<2; i++){putc('\n', plik);}
	for(int i=0; i<rb; i++){
		putc(bledy[i], plik);
	}
	for(int i=2; i<dlugosc_miejsca; i++){putc('\n', plik);}
}

int main(int argc, char *argv[]){
	if(argc==1){printf("brak sciezki do pliku");} return 0;
//wczytywanie i przerabianie sciezek do pliku
	int rozmiar_scin = strlen(argv[1]);  //rozmiar œcie¿ki wejsciowej
	int rozmiar_scout;  //rozmiar œcie¿ki wyjsciowej
	char sciezka_in[rozmiar_scin];
	for(int i=0; i<rozmiar_scin; i++){sciezka_in[i]=*(argv[1]+i);}
	if(argc==2){rozmiar_scout = rozmiar_scin+4;}
	else{rozmiar_scout = strlen(argv[2]);}
	char sciezka_out[rozmiar_scout];
	if(argc==2){
		for(int i=0; i<rozmiar_scin; i++){sciezka_out[i]=*(argv[1]+i);}
		int indeks=rozmiar_scin-1;
		while(sciezka_out[indeks]!='\\'){indeks--;}
		indeks++;
		for(int i=indeks; i<=rozmiar_scin; i++){
			sciezka_out[i+4]=sciezka_in[i];
		}
		sciezka_out[indeks]='o'; sciezka_out[indeks+1]='u'; sciezka_out[indeks+2]='t'; sciezka_out[indeks+3]='_';
	}
	else{
		for(int i=0; i<rozmiar_scout; i++){sciezka_out[i]=*(argv[2]+i);}
	}
	
//wczytywanie fila do tablicy o nazwie plik
	FILE* file;
	file = fopen(sciezka_in, "r");
	
	char* plik;
	int rozmiar_pliku=0;
	int rozmiar_buffora_pliku=1;
	plik = (char*) malloc(rozmiar_buffora_pliku);
	wczytywanie_pliku_do_tablicy(file, &plik, &rozmiar_pliku, &rozmiar_buffora_pliku);
	fclose(file);
		
//przerabianie na schemat
	struct blok* schemat;
	int rozmiar_schematu=0;
	int rozmiar_buffora_schematu=1;
	schemat = (struct blok*) malloc(rozmiar_buffora_schematu*sizeof(schemat));
	przerabianie_na_schemat(plik, &schemat, &rozmiar_schematu, &rozmiar_buffora_schematu);
	
//wypisywanie schematu
	for(int i=0; i<rozmiar_schematu; i++){
		printf("%c\t%u\n", (schemat+i)->co, (schemat+i)->dlugosc);
	}
	
	
	
//liczenie pliku
	FILE* file_wynik;
	file_wynik = fopen(sciezka_out, "w");
	
	int indeks_pliku=0;
	int indeks_schematu=0;
	int indeks_co_wypisano=0;
	while(indeks_schematu<rozmiar_schematu){
		char znak = '0';  //0 - nic  z - zamiana systemow  reszta taka jaka jest
		unsigned char systemliczbowy0=-1;
		unsigned char systemliczbowy1=-1;
		
		//wczytywanie znaku
		while((schemat+indeks_schematu)->co!='z'&&(schemat+indeks_schematu)->co!='S'&&(schemat+indeks_schematu)->co!='N'&&indeks_schematu<rozmiar_schematu){indeks_pliku+=(schemat+indeks_schematu)->dlugosc; indeks_schematu++;}
		if((schemat+indeks_schematu)->co=='z'){wczytywanie_znaku(plik, indeks_pliku, (schemat+indeks_schematu)->dlugosc, &znak); indeks_pliku+=(schemat+indeks_schematu)->dlugosc; indeks_schematu++;}
		else if((schemat+indeks_schematu)->co=='S'){if(wczytywanie_systemu_liczbowego(plik, indeks_pliku, (schemat+indeks_schematu)->dlugosc, &systemliczbowy0)==1){goto wypisywanie;}; znak='z'; indeks_pliku+=(schemat+indeks_schematu)->dlugosc; indeks_schematu++;}
		else if((schemat+indeks_schematu)->co=='N'){/*indeks_pliku+=(schemat+indeks_schematu)->dlugosc; indeks_schematu++;*/ czyblad=2; rb=11; charcpy("brak danych"); goto wypisywanie;}
		else{break;}
		
		//wczytywanie systemu liczbowego
		while((schemat+indeks_schematu)->co!='s'&&(schemat+indeks_schematu)->co!='N'&&indeks_schematu<rozmiar_schematu){indeks_pliku+=(schemat+indeks_schematu)->dlugosc; indeks_schematu++;}
		if((schemat+indeks_schematu)->co=='s'){if(wczytywanie_systemu_liczbowego(plik, indeks_pliku, (schemat+indeks_schematu)->dlugosc, &systemliczbowy1)==1){goto wypisywanie;} indeks_pliku+=(schemat+indeks_schematu)->dlugosc; indeks_schematu++;}
		else if((schemat+indeks_schematu)->co=='N'){/*indeks_pliku+=(schemat+indeks_schematu)->dlugosc; indeks_schematu++;*/ czyblad=2; rb=11; charcpy("brak danych"); goto wypisywanie;}
		else{break;}
		if(znak!='z'){systemliczbowy0=systemliczbowy1;}
		
		//sprawdzanie ile jest 'N'
		int ileN=0;
		int indeks_N=indeks_schematu;
		while((schemat+indeks_N)->co!='z'&&(schemat+indeks_N)->co!='s'&&indeks_N<rozmiar_schematu){
			if((schemat+indeks_N)->co=='N'){ileN++;}
			indeks_N++;
		}
		
		//liczenie poszczególnych czêœci przykladów
		for(int iN=0; iN<ileN; iN++){
			unsigned char* x0;  //x0 - pierwszy argument
			int rozmiar_x0=0;
			int rozmiar_buffora_x0=1;
			x0 = (unsigned char*) malloc(rozmiar_buffora_x0);
			
			//wczytywanie pierwszego argumentu
			while((schemat+indeks_schematu)->co!='l'&&(schemat+indeks_schematu)->co!='N'&&indeks_schematu<rozmiar_schematu){indeks_pliku+=(schemat+indeks_schematu)->dlugosc; indeks_schematu++;}
			if((schemat+indeks_schematu)->co=='l'){wczytywanie_liczby(plik, indeks_pliku, (schemat+indeks_schematu)->dlugosc, &x0, &rozmiar_x0, &rozmiar_buffora_x0, systemliczbowy0); indeks_pliku+=(schemat+indeks_schematu)->dlugosc; indeks_schematu++;}
			else if((schemat+indeks_schematu)->co=='N'){/*indeks_pliku+=(schemat+indeks_schematu)->dlugosc; indeks_schematu++;*/ czyblad=1; rb=11; charcpy("brak danych"); goto wypisywanie;}
			else{break;}
			zamienianie_kolejnosci(x0, rozmiar_x0);
			
			//obliczanie i wczytywanie kolejnych argumentów
			while((schemat+indeks_schematu)->co!='N'&&indeks_schematu<rozmiar_schematu){
				
				unsigned char* x1;  //x1 - aktualny argument
				int rozmiar_x1=0;
				int rozmiar_buffora_x1=1;
				x1 = (unsigned char*) malloc(rozmiar_buffora_x1);
				
				//wczytywanie drugiego argumentu
				while((schemat+indeks_schematu)->co!='l'&&(schemat+indeks_schematu)->co!='N'&&indeks_schematu<rozmiar_schematu){indeks_pliku+=(schemat+indeks_schematu)->dlugosc; indeks_schematu++;}
				if((schemat+indeks_schematu)->co=='l'){wczytywanie_liczby(plik, indeks_pliku, (schemat+indeks_schematu)->dlugosc, &x1, &rozmiar_x1, &rozmiar_buffora_x1, systemliczbowy0); indeks_pliku+=(schemat+indeks_schematu)->dlugosc; indeks_schematu++;}
				else if((schemat+indeks_schematu)->co=='N'){indeks_pliku+=(schemat+indeks_schematu)->dlugosc; indeks_schematu++;}
				else{break;}
				zamienianie_kolejnosci(x1, rozmiar_x1);
				
				//wykonywanie obliczeñ
				switch(znak){
					case '+':
						dodawanie(&x0, &rozmiar_x0, &rozmiar_buffora_x0, x1, rozmiar_x1, systemliczbowy1);
						break;
					case '*':
						mnozenie(&x0, &rozmiar_x0, &rozmiar_buffora_x0, x1, rozmiar_x1, systemliczbowy1);
						break;
					case '/':
						dzielenie(&x0, &rozmiar_x0, &rozmiar_buffora_x0, x1, rozmiar_x1, systemliczbowy1);
						break;
					case '%':
						reszta_z_dzielenia(&x0, &rozmiar_x0, &rozmiar_buffora_x0, x1, rozmiar_x1, systemliczbowy1);
						break;
					case '^':
						potegowanie(&x0, &rozmiar_x0, &rozmiar_buffora_x0, &x1, &rozmiar_x1, &rozmiar_buffora_x1, systemliczbowy1);
						break;
				}
				
			}
			if(znak=='z'){zamienianie_systemow(&x0, &rozmiar_x0, &rozmiar_buffora_x0, systemliczbowy0, systemliczbowy1);}
			
			//wypisywanie wyniku i pliku od ostatniego wypisania
			wypisywanie:
			while((schemat+indeks_schematu)->co!='N'&&indeks_schematu<rozmiar_schematu){indeks_pliku+=(schemat+indeks_schematu)->dlugosc; indeks_schematu++;}  //gdy przerwano wykonywanie przykladu b³êdem i goto
				
			for(int i=indeks_co_wypisano; i<indeks_pliku; i++){
				putc(*(plik+i), file_wynik);
			}
			zamienianie_kolejnosci(x0, rozmiar_x0);
			if(czyblad==0){wypisywanie_liczby(file_wynik, x0, rozmiar_x0, (schemat+indeks_schematu)->dlugosc);}
			else{wypisywanie_bledu(file_wynik, (schemat+indeks_schematu)->dlugosc);}
			
			indeks_pliku+=(schemat+indeks_schematu)->dlugosc; indeks_schematu+=1;
			indeks_co_wypisano=indeks_pliku;
			
			if(czyblad==1){czyblad=0; rb=0;}
		}
		czyblad=0; rb=0;
	}
	
	return 0;
}
