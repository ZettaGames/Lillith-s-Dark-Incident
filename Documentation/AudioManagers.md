# Documentación de scripts para el manejo del audio

## SoundMixerManager.cs
Se encarga de controlar el volumen global, de efectos y de música a través de tres métodos distintos que son llamados desde los "sliders" en el menú de sonido.

## SoundFXManager.cs
Se encarga de los efectos de sonido. Crea una instancia de objeto para que pueda ser llamado desde cualquier otro script.
### PlaySoundFXClip()
El método recibe como parámetro un clip de audio, una posición en donde se instanciará el objeto que lo reproduzca y el volumen con el que sonará. Luego de que el audio es reproducido, se destruye el objeto creado. Debido a que el script es un singleton, este método puede ser llamado desde cualquier otro script.

## SoundButtonsManager.cs
El script tiene dos variables de clip de audio, una para el efecto de sonido al presionar un botón y otra para el efecto al cambiar de botón.
### Update()
Dentro de este método, se detecta continuamente si el objeto seleccionado actualmente es distinto al último objeto seleccionado; de ser así, se reproduce el efecto de cambio de botón mediante el método "PlaySoundFXClip()" en la instancia de "SoundFXManager.cs".
### PlaySoundButton()
El método es llamado al presionar los botones que deben reproducir el sonido. Al ser presionados, reproducen el efecto de sonido mediante el método "PlaySoundFXClip()" en la instancia de "SoundFXManager.cs".