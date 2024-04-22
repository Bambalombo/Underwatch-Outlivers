using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSwitcher : MonoBehaviour
{
    PlayerStatsController playerStatsController;
    public SpriteRenderer spriteRenderer;
    public Sprite elementalistSprite;
    public Sprite voidwalkerSprite;
    public Sprite mutantBerserkerSprite;
    public Sprite xenobiologistSprite;
    public Sprite XI_017Sprite;
    
    void Start()
    {
        playerStatsController = GetComponent<PlayerStatsController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetSprite(playerStatsController.playerClass);
    }

    public void Update()
    {
        FlipSprite();
    }

    private void FlipSprite()
    {
        if (Time.timeScale == 0)
            return;
        
        if (playerStatsController.GetLastMoveDirection().x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (playerStatsController.GetLastMoveDirection().x < 0)
        {
            spriteRenderer.flipX = true;
        }
    }
    
    public void SetSprite(PlayerStatsController.PlayerClass playerClass)
    {
        Sprite newSprite = null; // Initialize variable to hold the new sprite

        // Load the appropriate sprite based on the player class
        switch (playerClass)
        {
            case PlayerStatsController.PlayerClass.Elementalist:
                newSprite = elementalistSprite;
                break;
            case PlayerStatsController.PlayerClass.Voidwalker:
                newSprite = voidwalkerSprite;
                break;
            case PlayerStatsController.PlayerClass.MutantBerserker:
                newSprite = mutantBerserkerSprite;
                break;
            case PlayerStatsController.PlayerClass.Xenobiologist:
                newSprite = xenobiologistSprite;
                break;
            case PlayerStatsController.PlayerClass.XI_017:
                newSprite = XI_017Sprite;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(playerClass), playerClass, null);
        }

        // Set the spriteRenderer's sprite to the newly loaded sprite
        if (newSprite != null) // Check if the sprite was loaded successfully
        {
            spriteRenderer.sprite = newSprite;
        }
        else
        {
            Debug.LogError("Sprite could not be loaded.");
        }
    }

    
}
